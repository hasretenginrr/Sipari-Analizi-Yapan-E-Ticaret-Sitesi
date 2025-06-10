using BitirmeProjesi.BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.Controllers
{
    public class PriceController : Controller
    {
        private readonly ProductManager _productManager;

        public PriceController(ProductManager productManager)
        {
            _productManager = productManager;
        }
        public IActionResult Index(int min = 0, int max = 600)
        {
            var allProducts = _productManager.TGetList();

            var filteredProducts = allProducts
                .Where(p => p.price >= min && p.price <= max)
                .ToList();

            ViewBag.MinPrice = min;
            ViewBag.MaxPrice = max;
            ViewBag.Title = $"Fiyat Aralığı: {min} - {max} TL";

            return View(filteredProducts);
        }



    }
}
