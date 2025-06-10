using System.Security.Claims;
using BitirmeProjesi.BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.ViewComponents.Brands
{
    public class BrandsList : ViewComponent
    {
        private readonly ProductManager _productManager;

        public BrandsList(ProductManager productManager)
        {
            _productManager = productManager;
        }

        public IViewComponentResult Invoke()
        {
            var values = _productManager.TGetList();

            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.UserId = string.IsNullOrEmpty(userIdString) ? 0 : Convert.ToInt32(userIdString);

            return View(values);
        }
    }
}