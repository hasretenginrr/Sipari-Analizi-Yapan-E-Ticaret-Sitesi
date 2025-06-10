using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BitirmeProjesi.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
       
    }
}
