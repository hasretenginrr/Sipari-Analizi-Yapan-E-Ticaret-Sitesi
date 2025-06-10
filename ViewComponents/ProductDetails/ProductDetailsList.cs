using BitirmeProjesi.BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.ViewComponents.ProductDetails
{
    public class ProductDetailsList : ViewComponent
    {
        private readonly ProductManager _productManager;

        public ProductDetailsList(ProductManager productManager)
        {
            _productManager = productManager;
        }
        public IViewComponentResult Invoke(int id, int userId)
        {
            var product = _productManager.TGetByID(id);

            if (product == null)
                return Content("Ürün bulunamadı.");

            var model = new ProductDetailsViewModel
            {
                Product = product,
                UserId = userId, 
                
            };

            return View(model);
        }

    }
    public class ProductDetailsViewModel
    {
        public Products Product { get; set; }
        public int UserId { get; set; }
    }
}