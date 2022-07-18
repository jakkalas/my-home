using Microsoft.AspNetCore.Mvc;

namespace MyHomeApi.Controllers
{
    public class AuthorizeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
