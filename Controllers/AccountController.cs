using BitirmeProjesi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

     [HttpPost]
       public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
       {
           var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           var user = _context.Users.FirstOrDefault(u => u.user_id.ToString() == userId);

           if (user == null)
           {
               return RedirectToAction("Index", "Login");
           }

           if (user.password != currentPassword)
           {
               ModelState.AddModelError("", "Mevcut þifre yanlýþ.");
               return View();
           }

           if (newPassword != confirmPassword)
           {
               ModelState.AddModelError("", "Yeni þifreler eþleþmiyor.");
               return View();
           }

           user.password = newPassword;
           _context.SaveChanges();

           TempData["SuccessMessage"] = "Þifreniz baþarýyla güncellenmiþtir.";
           return RedirectToAction("Index", "Account");
       }




    [HttpPost]
    public IActionResult AddAddress(string adres)
    {
        if (string.IsNullOrWhiteSpace(adres))
        {
            ModelState.AddModelError("adres", "Adres boþ olamaz.");
           
            return RedirectToAction("Index", "Account");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            
            return RedirectToAction("Index", "Login");
        }

        var user = _context.Users.FirstOrDefault(u => u.user_id.ToString() == userId);
        if (user == null)
        {
            
            return RedirectToAction("Index", "Login");
        }

        user.Adress = adres;

        _context.SaveChanges();

        TempData["SuccessMessage"] = "Adres baþarýyla kaydedildi!";
        return RedirectToAction("Index", "Account");
    }




    public IActionResult Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _context.Users.FirstOrDefault(u => u.user_id.ToString() == userId);

        if (string.IsNullOrEmpty(userId) || user == null)
        {
            return RedirectToAction("Index", "Login");
        }

        ViewBag.CurrentUser = user;

        
        var orders = _context.Orders
                             .Where(o => o.user_id.ToString() == userId)
                             .ToList();

       
        var orderDetails = _context.Order_Details
                                   .Where(od => orders.Select(o => o.id).Contains(od.order_id))
                                   .Join(_context.Products,
                                         od => od.product_id,
                                         p => p.p_id,
                                         (od, p) => new { OrderDetail = od, Product = p })
                                   .ToList();

        var products = _context.Products.ToList();

        var model = new AccountViewModel
        {
            CurrentUser = user,
            Users = _context.Users.ToList(),
            Orders = orders,
            Order_Details = orderDetails.Select(od => od.OrderDetail).ToList(), 
            Products = products
        };

        return View(model);
    }



}
