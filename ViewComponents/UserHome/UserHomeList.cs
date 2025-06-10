using Microsoft.AspNetCore.Mvc;
using BitirmeProjesi.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BitirmeProjesi.BusinessLayer.Concrete;

namespace BitirmeProjesi.ViewComponents.UserHome
{
    public class UserHomeList : ViewComponent
    {
        private readonly ProductManager _productManager;
        public UserHomeList(ProductManager productManager) // Bağımlılık Enjeksiyonu
        {
            _productManager = productManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = _productManager.TGetList(); // Ürünler

            var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                Console.WriteLine("⚠️ Uyarı: Giriş yapan kullanıcının ID bilgisi alınamadı.");
                // Kullanıcı ID alınamazsa bile ürünleri modele koy
                var fallbackModel = new UserHomeViewModel
                {
                    Products = values
                };
                return View(fallbackModel);
            }

            var model = new UserHomeViewModel
            {
                Products = values
                // İstersen Users veya Recommendations da doldurabilirsin
            };

            return View(model);
        }

    }
}

