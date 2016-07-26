using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using AP.RMA.Frontend.CA.Models;
using System.Diagnostics;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.RMA.Frontend.CA.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private DatabaseContext dbContext;

        public MainController(DatabaseContext context)
        {
            dbContext = context;
        }


        // GET: /<controller>/
        [RequireHttps]
        public IActionResult Index()
        {

            Debug.WriteLine($"Role: {User.IsInRole("Admin")}");
            LoginModel user = new LoginModel();
            //user.Login = User.Identity.Name;
            user = dbContext.Users.First(u => u.Login == User.Identity.Name);
            Debug.WriteLine($"Name: {User.Identity.Name}");
            //user.Id = dbContext.Users.First(i => i.UserName == User.Identity.Name).Id;
            //user.Uid = User.Claims.First(x => x.Type == "UID").Value;
           /* if (User.Claims.Any(x => x.Type.ToString() == ClaimValueTypes.Sid.ToString()))
            {
                user.Id = User.Claims.First(x => x.Type.ToString() == ClaimValueTypes.Sid.ToString()).Value;
            }*/
            //List<Claim> claims = User.Claims.ToList();
            /*foreach (Claim claim in User.Claims)
            {
                Debug.WriteLine($"Type: {claim.Type}");
                Debug.WriteLine($"Subject: {claim.OriginalIssuer}");
                Debug.WriteLine($"Value: {claim.Value}");
                Debug.WriteLine($"Issuer: {claim.Issuer}");
                Debug.WriteLine($"Tyep: {ClaimTypes.Sid}");



            }*/

            user.IsAdmin = User.IsInRole("Admin");
            //Context
            return View(user);
        }
    }
}
