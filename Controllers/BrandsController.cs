using System.Security.Claims;
using BitirmeProjesi.BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ProductManager _productManager;

        public BrandsController(ProductManager productManager)
        {
            _productManager = productManager;
        }

        public IActionResult Index(string brandName)
        {
            var allProducts = _productManager.TGetList();
            var filteredProducts = allProducts
                .Where(p => p.brand == brandName)
                .ToList();

            ViewBag.BrandName = brandName;

            // Kullanıcının giriş yapıp yapmadığını kontrol et
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.UserId = string.IsNullOrEmpty(userIdString) ? 0 : Convert.ToInt32(userIdString);

            return View(filteredProducts);
        }
    }
}