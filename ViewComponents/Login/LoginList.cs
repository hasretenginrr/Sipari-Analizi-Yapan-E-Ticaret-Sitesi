using Microsoft.AspNetCore.Mvc;

[ViewComponent]  
public class LoginListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View(); 
    }
}
