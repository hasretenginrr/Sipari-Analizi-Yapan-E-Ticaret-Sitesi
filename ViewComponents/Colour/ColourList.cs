using System.Security.Claims;
using BitirmeProjesi.BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.ViewComponents.Brands
{
    public class ColourList : ViewComponent
    {
        private readonly ProductManager _productManager;

        public ColourList(ProductManager productManager)
        {
            _productManager = productManager;
        }

        public IViewComponentResult Invoke()
        {
            var values = _productManager.TGetList();

            // Kullanıcı kimliği alınıyor
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdString))
            {
                ViewBag.UserId = Convert.ToInt32(userIdString);
            }
            else
            {
                ViewBag.UserId = 0;
            }

            return View(values);
        }
    }
}