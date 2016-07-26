using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using AP.RMA.Frontend.CA.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.RMA.Frontend.CA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private DatabaseContext dbContext;

        public AdminController(DatabaseContext context)
        {
            dbContext = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            AdminViewModel view = new AdminViewModel();
            view.Users = dbContext.Users.ToList();
            view.Cores = dbContext.Cores.ToList();
            return View(view);
        }
    }
}
