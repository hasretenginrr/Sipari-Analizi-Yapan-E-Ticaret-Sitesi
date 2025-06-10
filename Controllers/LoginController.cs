using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using BitirmeProjesi.DataAccessLayer;
using BitirmeProjesi.DTOs;
using BitirmeProjesi.Models;

public class LoginController : Controller
{
    private readonly AppDbContext _context;

    public LoginController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

   
    [HttpPost]
    public async Task<IActionResult> Index(LoginDtos loginDtos)
    {
        var user = _context.Users.FirstOrDefault(x => x.email == loginDtos.Email);

        if (user == null || user.password != loginDtos.Password) 
        {
            TempData["ErrorMessage"] = "Hatalı e-posta veya sifre, lutfen tekrar deneyiniz!";
            return View();
        }

        await SignInUser(user);
        return RedirectToAction("Index", "UserHome");
    }

    [HttpGet]
    public IActionResult Signup()
    {
        return View();
    }

    // **KAYIT OLMA (Signup)**
    [HttpPost]
    public async Task<IActionResult> Signup(RegisterDTOs registerDTOs)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index", "Login");
        }

        var existingUser = _context.Users.FirstOrDefault(x => x.email == registerDTOs.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı!");
            return RedirectToAction("Index", "Login");
        }

        var newUser = new Users
        {
            user_name = registerDTOs.Name,
            email = registerDTOs.Email,
            password = registerDTOs.Password, 
            gender = registerDTOs.Gender
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        await SignInUser(newUser); 
        return RedirectToAction("Index", "UserHome");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }

  
    private async Task SignInUser(Users user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()),
            new Claim(ClaimTypes.Name, user.user_name),
            new Claim(ClaimTypes.Email, user.email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}
