using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using AP.SecuritySystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.SecuritySystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            IAuthorizationService auth)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            _auth = auth;

        }

        public UserManager<ApplicationUser> UserManager
        {
            get;
            private set;
        }

        public SignInManager<ApplicationUser> SignInManager
        {
            get;
            private set;
        }

        public RoleManager<IdentityRole> RoleManager
        {
            get;
            private set;
        }

        private readonly IAuthorizationService _auth;


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            #region Create User&Role

            /*
            ApplicationUser newuser = new ApplicationUser { UserName = "aydnep", Email = "aydnep@aydnep.com.ua", EmailConfirmed = true };
            
            var role = await RoleManager.CreateAsync(new IdentityRole("admin"));
            var user = await UserManager.CreateAsync(newuser, model.Password);
            var usertorole = await UserManager.AddToRoleAsync(newuser, "admin");

            role = await RoleManager.CreateAsync(new IdentityRole("test1"));
            role = await RoleManager.CreateAsync(new IdentityRole("test2"));
            role = await RoleManager.CreateAsync(new IdentityRole("test3"));

            var claim = await RoleManager.AddClaimAsync(RoleManager.Roles.First(r => r.Name == "admin"), new Claim("AccessLevel", "admin"));

            */
            #endregion
            ViewBag.ReturnUrl = returnUrl;
            // Validate model values
            if (ModelState.IsValid)
            {
                // if Model is OK than we Check login/password
                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //Claim claim = new Claim("admin","admin");
                    //RoleManager.AddClaimAsync()
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password!");
                    return View(model);
                }
            }
            // If we got this far, something failed - redisplay the form
            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #region User Management
        [HttpPost]
        [Authorize("Supervisor")]
        public async Task<IActionResult> UserCreate(AddUserModel user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            Console.WriteLine("Username: {0}, email: {1}, password: {2}, assigned roles: {3}", user.Username, user.Email, user.Password, user.AssignedRoles.Count);
            IdentityResult result;
            if (ModelState.IsValid)
            {

                ApplicationUser newuser = new ApplicationUser { UserName = user.Username, Email = user.Email, EmailConfirmed = true };

                //var role = await RoleManager.CreateAsync(new IdentityRole("admin"));
                result = await UserManager.CreateAsync(newuser, user.Password);
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                    return Json(new JsonModel { Result = false, Errors = result.Errors });
                }
                else
                {
                    if (user.AssignedRoles.Count > 0)
                    {
                        List<IdentityRole> assignedRoles = new List<IdentityRole>();
                        foreach (string roleId in user.AssignedRoles)
                        {
                            if (!(string.IsNullOrEmpty(roleId) || string.IsNullOrWhiteSpace(roleId)))
                            {
                                assignedRoles.Add(await RoleManager.FindByIdAsync(roleId));
                            }

                        }
                        if (assignedRoles.Count > 0)
                        {
                            result = await UserManager.AddToRolesAsync(newuser, assignedRoles.Select(r => r.Name).AsEnumerable());
                            if (!result.Succeeded)
                            {
                                return Json(new JsonModel { Result = false, Errors = result.Errors });
                            }

                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Model error!");
                errors.Add(new IdentityError { Code = "8086", Description = "Model error!" });
                return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
            }
            Console.WriteLine(Json(new JsonModel { Result = true }).Value.ToString());
            return Json(new JsonModel { Result = true });

        }

        [HttpPost]
        [Authorize("Supervisor")]
        public async Task<IActionResult> UserEdit(EditUserModel selectedUser)
        {
            IdentityResult result;
            List<IdentityError> errors = new List<IdentityError>();
            if (ModelState.IsValid)
            {
                ApplicationUser user = UserManager.Users.First(u => u.Id == selectedUser.Id);
                IEnumerable<string> roles = await UserManager.GetRolesAsync(user);
                result = await UserManager.RemoveFromRolesAsync(user, roles);
                if (!result.Succeeded)
                {
                    return Json(new JsonModel { Result = false, Errors = result.Errors });
                }

                List<IdentityRole> assignedRoles = new List<IdentityRole>();
                foreach (string roleId in selectedUser.AssignedRoles)
                {
                    if (!(string.IsNullOrEmpty(roleId) || string.IsNullOrWhiteSpace(roleId)))
                    {
                        assignedRoles.Add(await RoleManager.FindByIdAsync(roleId));
                    }
                }
                user.Email = selectedUser.Email;
                if (assignedRoles.Count > 0)
                {
                    result = await UserManager.AddToRolesAsync(user, assignedRoles.Select(r => r.Name).AsEnumerable());
                    if (!result.Succeeded)
                    {
                        return Json(new JsonModel { Result = false, Errors = result.Errors });
                    }

                }
                result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return Json(new JsonModel { Result = false, Errors = result.Errors });
                }
            }
            else
            {
                Console.WriteLine("Model error!");
                errors.Add(new IdentityError { Code = "8086", Description = "Model error!" });
                return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
            }

            return Json(new JsonModel { Result = true });
        }

        [HttpPost]
        [Authorize("Supervisor")]
        public async Task<IActionResult> UserDelete(string id)
        {
            Console.WriteLine("Delete {0}", id);

            ApplicationUser user = await UserManager.FindByIdAsync(id);
            IdentityResult result = await UserManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Json(new JsonModel { Result = false, Errors = result.Errors });
            }
            return Json(new JsonModel { Result = true });
        }

        [HttpPost]
        [Authorize("Supervisor")]
        public async Task<IActionResult> ForgotPassword(string id)
        {
            Console.WriteLine();
            Console.WriteLine("Generate url for {0}", id);
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            var url = await UserManager.GeneratePasswordResetTokenAsync(user);
            Console.WriteLine();
            Console.WriteLine(url);
            Console.WriteLine();
            var callbackUrl = Url.Action("ResetPassword", "Account", new { id = user.Id, token = url }, Request.Scheme, null);
            Console.WriteLine(callbackUrl);
            List<IdentityError> errors = new List<IdentityError>();
            errors.Add(new IdentityError { Code = "8086", Description = callbackUrl });

            return Json(new JsonModel { Result = true, Errors = errors });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            Console.WriteLine();
            Console.WriteLine("ResetPassword {0} \r\nToken: {1}", id, token);
            try
            {
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                return View(new ResetPasswordModel { User = user, Token = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string id, string token, string password)
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine("New password is {0}\r\nToken: {1}", password, token);
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                IdentityResult result = await UserManager.ResetPasswordAsync(user, token, password);
                if (!result.Succeeded)
                {
                    return Json(new JsonModel { Result = false, Errors = result.Errors });
                }
                List<IdentityError> errors = new List<IdentityError>();
                errors.Add(new IdentityError { Code = "8086", Description = "Model error!" });

                return Json(new JsonModel { Result = true });
            }
            catch (Exception ex)
            {
                List<IdentityError> errors = new List<IdentityError>();
                Console.WriteLine("Model error!");
                errors.Add(new IdentityError { Code = "8086", Description = ex.Message });
                return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
            }
        }
        #endregion

        #region Helpers
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}
