using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class OrderController : Controller
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmOrder()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
            return RedirectToAction("Index", "Login");

        var userId = Convert.ToInt32(userIdString);

        var cartItems = await _context.Cart
            .Where(c => c.user_id == userId)
            .ToListAsync();

        if (cartItems == null || !cartItems.Any())
        {
            TempData["ErrorMessage"] = "Sepetiniz boş.";
            return RedirectToAction("Index", "Cart");
        }

        // 1. Orders tablosuna kayıt
        var order = new Orders
        {
            user_id = userId,
            order_date = DateTime.UtcNow
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); // order_id oluşturulsun

        // 2. Order_Details tablosuna kayıt
        foreach (var item in cartItems)
        {
            var orderDetail = new Order_Details
            {
                order_id = order.id,
                product_id = item.product_id,
                quantity = item.quantity ?? 1
            };
            _context.Order_Details.Add(orderDetail);
        }

        // Sepeti temizle
        _context.Cart.RemoveRange(cartItems);

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Siparişiniz başarıyla alındı.";
        return RedirectToAction("Index", "UserHome");
    }
}