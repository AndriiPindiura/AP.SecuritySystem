using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.RMA.Frontend.CA.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [RequireHttps]
        [AllowAnonymous]

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Main");
            }
            ViewBag.Title = "Welcome to ASP vNext!";
            return View();
        }

    }

}
