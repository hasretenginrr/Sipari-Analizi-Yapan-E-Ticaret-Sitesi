
using Microsoft.AspNetCore.Mvc;
using BitirmeProjesi.DataAccessLayer;
using BitirmeProjesi.Models;
using BitirmeProjesi.DTO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BitirmeProjesi.Controllers
{
    public class UserHomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserHomeController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendations(int userId)
        {
            try
            {
                string externalApiUrl = $"http://localhost:5062/recommend/{userId}";

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(externalApiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { error = "Dış servisten veri alınamadı." });
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RecommendResponse>(json);

                return Json(result?.Recommendations ?? new List<ProductDto>());
            }
            catch (Exception ex)
            {
                return Json(new { error = "Öneriler alınırken bir hata oluştu." });
            }
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.user_id == Convert.ToInt32(userId));
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.CurrentUser = user;
            ViewBag.UserId = user.user_id; 

            var model = new UserHomeViewModel
            {
                Users = user,
                Products = _context.Products?.ToList() ?? new List<Products>(),
                Recommendations = new List<ProductDto>() 
            };

            return View(model);
        }
    }
}

