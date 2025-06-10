using BitirmeProjesi.BusinessLayer.Concrete;
using BitirmeProjesi.DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.ViewComponents.Index
{
    public class IndexList : ViewComponent
    {
        private readonly ProductManager _productManager;

        public IndexList(ProductManager productManager) // Bağımlılık Enjeksiyonu
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
