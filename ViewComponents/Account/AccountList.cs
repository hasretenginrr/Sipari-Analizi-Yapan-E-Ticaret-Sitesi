using BitirmeProjesi.DTOs;
using BitirmeProjesi.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace proje3.ViewComponents.Account
{
    public class AccountList : ViewComponent
    {
        private readonly AppDbContext _context;

        public AccountList(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                // Kullanýcý ID'si yoksa boþ bir model döndür
                return View(new AccountViewModel());
            }

            // Kullanýcýyý buluyoruz
            var user = _context.Users.FirstOrDefault(u => u.user_id == Convert.ToInt32(userId));

            if (user == null)
            {
                // Kullanýcý bulunamazsa, boþ bir model döndür
                return View(new AccountViewModel());
            }

            // Kullanýcýya ait sipariþleri alýrken, Order_Details ile ürünleri JOIN yapýyoruz
            var orders = _context.Orders
                                 .Where(o => o.user_id == user.user_id)
                                 .ToList();

            // Order_Details ve ilgili Products tablosu arasýnda JOIN iþlemi yapýyoruz
            var orderDetailsWithProducts = (from od in _context.Order_Details
                                            join p in _context.Products on od.product_id equals p.p_id
                                            where orders.Select(o => o.id).Contains(od.order_id)
                                            select new
                                            {
                                                od,
                                                Product = p
                                            }).ToList();

            // Order_Details ve ilgili ürünler
            var orderDetails = orderDetailsWithProducts.Select(x => x.od).ToList();
            var products = orderDetailsWithProducts.Select(x => x.Product).ToList();

            // Modeli oluþturuyoruz
            var model = new AccountViewModel
            {
                CurrentUser = user,
                Users = _context.Users.ToList(),
                Orders = orders,
                Order_Details = orderDetails,
                Products = products,
            };

            // Modeli View'e gönderiyoruz
            return View(model);
        }
    }
}
