using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using BitirmeProjesi.BusinessLayer.Concrete; // ProductManager'ý ekle

public class ProductDetailsController : Controller
{
    private readonly ProductManager _productManager;

    public ProductDetailsController(ProductManager productManager)
    {
        _productManager = productManager;
    }

    public IActionResult ProductDetails(int id)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = 0;
        if (!string.IsNullOrEmpty(userIdStr))
        {
            userId = int.Parse(userIdStr);
        }

        var product = _productManager.TGetByID(id);
        if (product == null)
        {
            return NotFound();
        }

        // UserId’yi de ViewComponent’a parametre olarak gönderiyoruz
        return ViewComponent("ProductDetailsList", new { id = id, userId = userId });
    }

}