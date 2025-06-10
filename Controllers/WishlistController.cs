using System.Security.Claims;
using BitirmeProjesi.ViewComponents.Cart;
using BitirmeProjesi.ViewComponents.ProductDetails;
using BitirmeProjesi.ViewComponents.Wishlist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Ekledik

public class WishlistController : Controller
{
    private readonly AppDbContext _context;

    public WishlistController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> FavorilereEkle(int productId, int userId)
    {
        if (productId == 0 || userId == 0)
        {
            TempData["ErrorMessage"] = "Ürün veya Kullanıcı ID geçersiz.";
            return RedirectToAction("Index", "UserHome");
        }

        var productExists = await _context.Products.AnyAsync(p => p.p_id == productId);
        if (!productExists)
        {
            TempData["ErrorMessage"] = "Ürün bulunamadı.";
            return RedirectToAction("Index", "UserHome");
        }

        var existingItem = await _context.Wishlist
            .FirstOrDefaultAsync(c => c.user_id == userId && c.product_id == productId);

        if (existingItem == null)
        {
            var wishlistItem = new Wishlist
            {
                user_id = userId,
                product_id = productId
            };

            _context.Wishlist.Add(wishlistItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", "UserHome");
    }

    public async Task<IActionResult> Index()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Index", "Login");
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
    [HttpPost]
    public async Task<IActionResult> RemoveFromWishlist(int productId)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Index", "Login");
        }

        var userId = Convert.ToInt32(userIdString);

        var wishlistItem = await _context.Wishlist
            .FirstOrDefaultAsync(c => c.user_id == userId && c.product_id == productId);

        if (wishlistItem != null)
        {
            _context.Wishlist.Remove(wishlistItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

}
