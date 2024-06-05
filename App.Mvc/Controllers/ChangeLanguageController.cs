using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class ChangeLanguageController : Controller
    {
        //[Route("api/Change-Lang")]
        public IActionResult Index(
            [FromQuery] string lang
        )
        {
            Response.Cookies.Append("lang", lang);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""));
        }
    }
}