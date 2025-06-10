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
                // Kullan�c� ID'si yoksa bo� bir model d�nd�r
                return View(new AccountViewModel());
            }

            // Kullan�c�y� buluyoruz
            var user = _context.Users.FirstOrDefault(u => u.user_id == Convert.ToInt32(userId));

            if (user == null)
            {
                // Kullan�c� bulunamazsa, bo� bir model d�nd�r
                return View(new AccountViewModel());
            }

            // Kullan�c�ya ait sipari�leri al�rken, Order_Details ile �r�nleri JOIN yap�yoruz
            var orders = _context.Orders
                                 .Where(o => o.user_id == user.user_id)
                                 .ToList();

            // Order_Details ve ilgili Products tablosu aras�nda JOIN i�lemi yap�yoruz
            var orderDetailsWithProducts = (from od in _context.Order_Details
                                            join p in _context.Products on od.product_id equals p.p_id
                                            where orders.Select(o => o.id).Contains(od.order_id)
                                            select new
                                            {
                                                od,
                                                Product = p
                                            }).ToList();

            // Order_Details ve ilgili �r�nler
            var orderDetails = orderDetailsWithProducts.Select(x => x.od).ToList();
            var products = orderDetailsWithProducts.Select(x => x.Product).ToList();

            // Modeli olu�turuyoruz
            var model = new AccountViewModel
            {
                CurrentUser = user,
                Users = _context.Users.ToList(),
                Orders = orders,
                Order_Details = orderDetails,
                Products = products,
            };

            // Modeli View'e g�nderiyoruz
            return View(model);
        }
    }
}
