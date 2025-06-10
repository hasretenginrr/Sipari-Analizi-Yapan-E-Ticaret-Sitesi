 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BitirmeProjesi.ViewComponents.Cart
{
    public class CartList : ViewComponent
    {
        private readonly AppDbContext _context;

        public CartList(AppDbContext context)
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
            decimal tax = cartTotal * 0.18m; 
            decimal shipping = cartTotal >= 100 ? 0 : 15; 
            decimal total = cartTotal + tax + shipping;

            // Modeli oluştur ve gönder
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
    }
    
    public class CartItemViewModel
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int ProductID { get; set; }
        public int quantity { get; set; }

        public List<CartItemViewModel> CartItems { get; set; } 
        public decimal CartTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
    }
}