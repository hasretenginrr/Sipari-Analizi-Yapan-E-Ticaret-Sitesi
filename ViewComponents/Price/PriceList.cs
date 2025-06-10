using BitirmeProjesi.BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.ViewComponents.Price
{
    public class PriceList : ViewComponent
    {
        private readonly ProductManager _productManager;
        public PriceList(ProductManager productManager)
        {
            _productManager = productManager;
        }
        public IViewComponentResult Invoke()
        {
            var values = _productManager.TGetList();
            return View(values);
        }
    }
}
