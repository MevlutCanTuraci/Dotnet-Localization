#region Imports
using App.Mvc.Infrastructer.Helper.Localize;
using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
#endregion


namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILocalize _l;

        public HomeController(ILogger<HomeController> logger, ILocalize l)
        {
            _logger = logger;
            _l = l;
        }


        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
