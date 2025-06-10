using BitirmeProjesi.BusinessLayer.Concrete;
using BitirmeProjesi.DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.ViewComponents.Index
{
    public class UserList : ViewComponent
    {
        private readonly UserManager _userManager;

        public UserList(UserManager userManager) // Bağımlılık Enjeksiyonu
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var values = _userManager.TGetList();
            return View(values);
        }
    }
}
