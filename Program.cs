using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using System.Text.Json;
using System.Text.Json.Serialization;
using BitirmeProjesi.BusinessLayer.Concrete;
using BitirmeProjesi.DataAccessLayer;
using BitirmeProjesi.DataAccessLayer.Abstract;
using BitirmeProjesi.DataAccessLayer.EntityFramework;
//using BitirmeProjesi.ViewComponents.UserHome;
using proje3;
using DataAccessLayer.Abstract;
//using BitirmeProjesi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DataPreparation>();
builder.Services.AddSingleton<MLContext>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<ProductManager>();
builder.Services.AddScoped<IProductDal, EfProductDal>();
builder.Services.AddScoped<IUserDal, EfUserDal>();
//builder.Services.AddHttpClient<RecommendationService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<FeatureVectorCache>();




builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.WithOrigins("https://localhost:7086")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();
app.UseCors("AllowLocalhost");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Default}/{action=Index}/{id?}"
    );

    endpoints.MapGet("/recommend/{userId:int}", async (
        int userId,
        AppDbContext context,
        DataPreparation dataPreparation,
        MLContext mlContext) =>
    {
        var products = await context.Products.ToListAsync();

        var purchasedProducts = await context.Orders
            .Where(o => o.user_id == userId)
            .Join(context.Order_Details,
                order => order.id,
                detail => detail.order_id,
                (order, detail) => detail.product_id)
            .Distinct()
            .Join(context.Products,
                pid => pid,
                p => p.p_id,
                (pid, p) => p)
            .ToListAsync();

        if (!purchasedProducts.Any())
        {
            Console.WriteLine($"[INFO] Kullanıcı ({userId}) için satın alınan ürün bulunamadı.");
            return Results.Json(new
            {

                RecommendedProducts = new List<Products>()
            }, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            });
        }

        var recommendedProducts = dataPreparation.GetSimilarProductsByContent(userId, products, mlContext);

        var filteredRecommendations = recommendedProducts
            .Where(p => !purchasedProducts.Any(up => up.p_id == p.p_id))
            .Distinct()
            .Take(6)
            .ToList();

        return Results.Json(new
        {

            RecommendedProducts = filteredRecommendations
        }, new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        });
    });
});

app.Run();