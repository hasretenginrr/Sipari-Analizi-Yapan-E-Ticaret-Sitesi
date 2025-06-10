using System.Security.Claims;
using BitirmeProjesi.ViewComponents.Cart;
using BitirmeProjesi.ViewComponents.ProductDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Ekledik

public class CartController : Controller
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> SepeteEkle(int productId, int userId)
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

        var existingItem = await _context.Cart
            .FirstOrDefaultAsync(c => c.user_id == userId && c.product_id == productId);

        if (existingItem == null)
        {
            var cartItem = new Cart
            {
                user_id = userId,
                product_id = productId,
                quantity = 1
            };

            _context.Cart.Add(cartItem);
        }
        else
        {
            existingItem.quantity += 1;
            _context.Cart.Update(existingItem);
        }

        await _context.SaveChangesAsync();
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

        // Sepetteki ürünleri al
        var items = await _context.Cart
            .Include(c => c.Product)
            .Where(c => c.user_id == userId)
            .ToListAsync();

        
        var cartItems = items.Select(c => new CartItemViewModel
        {
            ProductName = c.Product.name,
            ProductPrice = (decimal)c.Product.price,
            ProductImage = c.Product.img,
            ProductID = c.Product.p_id,
            quantity = c.quantity ?? 1,
        }).ToList();

        decimal cartTotal = cartItems.Sum(x => x.ProductPrice * x.quantity);
        decimal tax = cartTotal * 0.18m; // %18 KDV
        decimal shipping = cartTotal >= 100 ? 0 : 15; // 100 TL üzeri ücretsiz kargo
        decimal total = cartTotal + tax + shipping;

        
        var model = new CartItemViewModel
        {
            CartItems = cartItems,
            CartTotal = cartTotal,
            Tax = tax,
            ShippingCost = shipping,
            Total = total
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Index", "Login");
        }

        var userId = Convert.ToInt32(userIdString);

        var item = await _context.Cart
            .FirstOrDefaultAsync(c => c.user_id == userId && c.product_id == productId);

        if (item != null)
        {
            _context.Cart.Remove(item);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Ürün sepetten kaldırıldı.";
        }
        else
        {
            TempData["ErrorMessage"] = "Ürün sepetinizde bulunamadı.";
        }

        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int productId, int change)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Index", "Login");
        }

        var userId = Convert.ToInt32(userIdString);

        var item = await _context.Cart
            .FirstOrDefaultAsync(c => c.user_id == userId && c.product_id == productId);

        if (item != null)
        {
            item.quantity += change;

           
            if (item.quantity <= 0)
            {
                _context.Cart.Remove(item);
            }
            else
            {
                _context.Cart.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }




}
