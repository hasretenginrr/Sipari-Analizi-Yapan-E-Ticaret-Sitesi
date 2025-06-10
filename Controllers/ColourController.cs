using System.Security.Claims;
using BitirmeProjesi.BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc; // eklenmeli

public class ColourController : Controller
{
    private readonly ProductManager _productManager;

    public ColourController(ProductManager productManager)
    {
        _productManager = productManager;
    }

    public IActionResult Index(string colourName)
    {
        var allProducts = _productManager.TGetList();
        var filteredProducts = allProducts
            .Where(p => p.colour == colourName)
            .ToList();

        ViewBag.ColourName = colourName;

        // Kullanıcı kimliğini al
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userIdString))
        {
            ViewBag.UserId = Convert.ToInt32(userIdString);
        }
        else
        {
            ViewBag.UserId = 0;
        }

        return View(filteredProducts);
    }
}