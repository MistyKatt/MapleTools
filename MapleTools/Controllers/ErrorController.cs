using Microsoft.AspNetCore.Mvc;

namespace MapleTools.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [Route("notfound")]
        public IActionResult Index()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
