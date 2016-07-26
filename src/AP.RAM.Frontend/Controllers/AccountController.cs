using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Authentication.Cookies;
using AP.RMA.Frontend.CA.Models;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNet.Cryptography.KeyDerivation;
using System.Linq;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.RMA.Frontend.CA.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private DatabaseContext dbContext;

        public AccountController(DatabaseContext context)
        {
            dbContext = context;
        }

        [RequireHttps]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [RequireHttps]
        [AllowAnonymous]
        [HttpGet]
        [Route("Account/CreateUser", Name = "NewUser")]
        public IActionResult CreateUser(string uid)
        {
            if (uid == AppDomain.CurrentDomain.GetData("newUserId").ToString())
            {
                //return HttpNotFound();
                return View();

            }
            return HttpBadRequest();

        }

        [RequireHttps]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser(LoginModel cerdentials)
        {
            cerdentials.Uid = AppDomain.CurrentDomain.GetData("newUserId").ToString();
            if (dbContext.Users.Any(i => i.Login == cerdentials.Login))
            {
                return RedirectToAction("CreateUser", "Account", new { uid = cerdentials.Uid });
            }

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            cerdentials.Salt = Convert.ToBase64String(salt);
            cerdentials.Hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: cerdentials.Hash,
                        salt: Convert.FromBase64String(cerdentials.Salt),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8));
            cerdentials.IsAdmin = false;
            dbContext.Users.Add(cerdentials);
            dbContext.SaveChanges();
            AppDomain.CurrentDomain.SetData("newUserId", Guid.NewGuid());
            return RedirectToAction("Index", "Home");
        }

        [RequireHttps]
        [Authorize(Roles = "Admin")]
        public IActionResult GenerateUserLink()
        {
            string uid = Guid.NewGuid().ToString();
            if (dbContext.Users.Any(i => i.Uid == uid))
            {
                return RedirectToAction("GenerateUserLink", "Account");
            }
            AppDomain.CurrentDomain.SetData("newUserId", uid);
            ViewBag.url = Url.Link("NewUser", new { uid = AppDomain.CurrentDomain.GetData("newUserId") });
            return View();
        }

        [HttpPost]
        [RequireHttps]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginModel cerdentials)
        {
            if (IsValid(cerdentials))
            {

                var claims = new List<Claim>();
                claims.Add(new Claim("Name", cerdentials.Login));
                claims.Add(new Claim("UID", dbContext.Users.First(i => i.Login == cerdentials.Login).Uid));
                if (dbContext.Users.First(a => a.Login == cerdentials.Login).IsAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                //foreach ()
                /*
                {
                    //new Claim("ID", cerdentials.Id),
                    new Claim("Name", cerdentials.Login),
                    //new Claim("role", "admin")
                };*/
                var id = new ClaimsIdentity(claims, "local", "Name", ClaimTypes.Role);
                await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
                return RedirectToAction("Index", "Main");
            }

            return RedirectToAction("Index", "Home");

        }

        private bool IsValid(LoginModel cerdentials)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            Debug.WriteLine($"Login: {cerdentials.Login}");
            Debug.WriteLine($"UID: {Guid.NewGuid()}");
            Debug.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            LoginModel user = new LoginModel();
            user = dbContext.Users.First(u => u.Login == cerdentials.Login);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: cerdentials.Hash,
            salt: Convert.FromBase64String(user.Salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            Debug.WriteLine($"Hashed: {hashed}");
            if (hashed == user.Hash)
            {
                return true;
            }
            return false;
        }

    }
}
