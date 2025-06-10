using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BitirmeProjesi.ViewComponents.Wishlist
{
    public class WishlistList : ViewComponent
    {
        private readonly AppDbContext _context;

        public WishlistList(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdString = (User as ClaimsPrincipal)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Content("Kullanıcı bulunamadı.");
            }

            var userId = Convert.ToInt32(userIdString);

            var wishlistItems = await _context.Wishlist
                .Where(c => c.user_id == userId)
                .Select(c => new WishlistItemViewModel
                {
                    ProductName = c.Product.name,
                    ProductPrice = (decimal)c.Product.price,
                    ProductImage = c.Product.img,
                    ProductID = c.Product.p_id,
                })
                .ToListAsync();

            return View(wishlistItems);
        }
    }
    public class WishlistItemViewModel
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }

        public int ProductID { get; set; }
    }
}