#region Imports
using App.Api.Infrastructer.Helper.Localize;
using Microsoft.AspNetCore.Mvc;
#endregion


namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        #region DI Implementations

        private readonly ILocalize _l;

        public HomeController(ILocalize l)
        {
            _l = l;
        }

        #endregion


        [HttpGet("Say-Hello")]
        public async Task<IActionResult> HelloAsync(
            [FromQuery] string key = "Hello"
        )
        {
            //OR
            //string value = GetValue(key);

            string value = _l[key];
            return Ok(value);
        }



        [HttpGet("Say-Hi")]
        public async Task<IActionResult> HiAsync(
            [FromQuery] string key = "Hi"
        )
        {
            //OR
            //string value = GetValue(key);

            string value = _l[key];
            return Ok(value);
        }



        [HttpGet("Say-Whatsup")]
        public async Task<IActionResult> WhatsupAsync(
             [FromQuery] string key = "Whatsup"
        )
        {
            //OR
            //string value = GetValue(key);

            string value = _l[key];
            return Ok(value);
        }



        private string GetValue(string key)
        {
            return _l[key];
        }

    }
}