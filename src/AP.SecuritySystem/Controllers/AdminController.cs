using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using AP.SecuritySystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.SecuritySystem.Controllers
{
    [Authorize("Supervisor")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdminController(ApplicationDbContext dataContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAuthorizationService auth)
        {
            db = dataContext;
            UserManager = userManager;
            RoleManager = roleManager;
            _auth = auth;

        }

        private readonly IAuthorizationService _auth;

        public UserManager<ApplicationUser> UserManager
        {
            get;
            private set;
        }

        public RoleManager<IdentityRole> RoleManager
        {
            get;
            private set;
        }


        // GET: /<controller>/
        //[Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            ViewBag.Title = "AP Security System Configuration Page";
            //Console.WriteLine("Admin is loaded.");
            AdminModel counters = new AdminModel();
            counters.videoServersCount = db.Servers.Count(m => m.Mode == 0);
            counters.backupServersCount = db.Servers.Count(m => m.Mode == 1);
            counters.analyticsServersCount = db.Servers.Count(m => m.Mode == 2);
            counters.usersCount = db.Users.Count();
            counters.roleCount = db.Roles.Count();
            /*
            var users = db.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains(roleId))
                .ToList();
                */
            //var roles = db.CoreRoles.Select(r => r.Role);
            //roles.Where(x => x.)
            return View(counters);
        }

        public IActionResult GetAdminContent(string vc, int id, string title, string icon)
        {
            //Console.WriteLine("View Component is " + vc);
            //Console.WriteLine(RoleManager.Roles.ToList().First().Name);

            return ViewComponent(vc, new AdminVCModel { Title = title, Icon = icon, ServerType = id });
            //return PartialView();
        }

        public IActionResult UserEdit(string id)
        {
            //Console.WriteLine("Edit User");
            return ViewComponent("UserEdit", id);
        }

        public IActionResult EntryList(int id)
        {
            return ViewComponent("EntryList", id);
        }

        #region  RoleManager

        [HttpGet]
        public IActionResult RoleEdit(string id)
        {
            //Console.WriteLine("Edit Role");
            return ViewComponent("RoleEdit", id);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(EditRoleModel roleInfo)
        {
            IdentityResult result;
            List<IdentityError> errors = new List<IdentityError>();
            //Console.WriteLine("Id: {0}, counts: {1},{2}", roleInfo.Id, roleInfo.AssignedUsers.Count, roleInfo.AssignedServers.Count);
            var role = await RoleManager.FindByIdAsync(roleInfo.Id);
            try
            {
                /************* User in role *************/
                List<ApplicationUser> assignedUsers = new List<ApplicationUser>();
                foreach (string userId in roleInfo.AssignedUsers)
                {
                    if (!(string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId)))
                    {
                        var user = await UserManager.FindByIdAsync(userId);
                        assignedUsers.Add(user);
                        bool inRole = await UserManager.IsInRoleAsync(user, role.Name);
                        //Console.WriteLine("add is in role {0}", inRole);
                        if (!inRole)
                        {
                            //Console.WriteLine("Adding user {0} to role {1}", user.UserName, role.Name);
                            result = await UserManager.AddToRoleAsync(user, role.Name);
                            if (!result.Succeeded)
                            {
                                Console.WriteLine(result);
                                return Json(new JsonModel { Result = false, Errors = result.Errors });
                            }
                        }
                    }
                }

                IEnumerable<ApplicationUser> notAssignedUsers = UserManager.Users.ToList().Except(assignedUsers.AsEnumerable());

                foreach (ApplicationUser user in notAssignedUsers)
                {
                    bool inRole = await UserManager.IsInRoleAsync(user, role.Name);
                    //Console.WriteLine("remove is in role {0}", inRole);
                    if (inRole)
                    {
                        result = await UserManager.RemoveFromRoleAsync(user, role.Name);
                        if (!result.Succeeded)
                        {
                            Console.WriteLine(result);
                            return Json(new JsonModel { Result = false, Errors = result.Errors });
                        }
                    }
                }
                /************* Server in role *************/
                IEnumerable<ServersRoles> serversRoles = db.ServersRoles.ToList().Where(w => w.Role.Id == role.Id);
                foreach (ServersRoles relationship in serversRoles)
                {
                    db.ServersRoles.Remove(relationship);
                }
                db.SaveChanges();

                foreach (int serverId in roleInfo.AssignedServers)
                {
                    db.ServersRoles.Add(new ServersRoles { ServerId = serverId, Role = role });
                }
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errors.Add(new IdentityError { Code = "8086", Description = ex.Message });
                return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });

            }


            return Json(new JsonModel { Result = true });
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(EditRoleModel roleInfo)
        {
            IdentityResult result;
            List<IdentityError> errors = new List<IdentityError>();
            //Console.WriteLine("Role: {0}, counts: {1},{2}", roleInfo.Name, roleInfo.AssignedUsers.Count, roleInfo.AssignedServers.Count);
            result = await RoleManager.CreateAsync(new IdentityRole(roleInfo.Name));
            if (!result.Succeeded)
            {
                Console.WriteLine(result);
                return Json(new JsonModel { Result = false, Errors = result.Errors });
            }
            try
            {
                var role = await RoleManager.FindByNameAsync(roleInfo.Name);

                /************* User in role *************/
                foreach (string userId in roleInfo.AssignedUsers)
                {
                    if (!(string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId)))
                    {
                        var user = await UserManager.FindByIdAsync(userId);
                        //Console.WriteLine("Adding user {0} to role {1}", user.UserName, role.Name);
                        result = await UserManager.AddToRoleAsync(user, role.Name);
                        if (!result.Succeeded)
                        {
                            Console.WriteLine(result);
                            return Json(new JsonModel { Result = false, Errors = result.Errors });
                        }
                    }
                }

                /************* Server in role *************/
                foreach (int serverId in roleInfo.AssignedServers)
                {
                    db.ServersRoles.Add(new ServersRoles { ServerId = serverId, Role = role });
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errors.Add(new IdentityError { Code = "8086", Description = ex.Message });
                return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });

            }


            return Json(new JsonModel { Result = true });

        }

        [HttpPost]
        public async Task<IActionResult> RoleDelete(string id)
        {
            IdentityResult result;
            List<IdentityError> errors = new List<IdentityError>();
            //Console.WriteLine("Delete role {0}", id);
            try
            {


                var role = await RoleManager.FindByIdAsync(id);
                result = await RoleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    return Json(new JsonModel { Result = false, Errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Console.WriteLine(ex.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
                else
                {
                    Console.WriteLine(ex.InnerException.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.InnerException.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
            }
            return Json(new JsonModel { Result = true });


        }

        #endregion

        #region ServerManager
        [HttpGet]
        public IActionResult ServerEdit(string id)
        {
            //Console.WriteLine("Edit Server");
            return ViewComponent("ServerEdit", id);
        }

        [HttpPost]
        public async Task<IActionResult> ServerEdit(EditServerRole serverInfo)
        {
            List<IdentityError> errors = new List<IdentityError>();
            try
            {

                Console.WriteLine(serverInfo.Server.Id);
                if (serverInfo.Server.Id == -1)
                {
                    Console.WriteLine("New Server");
                }
                Console.WriteLine("Description:{0}\r\nIP:{1}\r\nMode:{2}\r\nConnectionString:{3}\r\nITVName:{4}\r\nAssignedRoles:{5}", serverInfo.Server.Description,
                    serverInfo.Server.Ip,
                    serverInfo.Server.Mode,
                    serverInfo.Server.ConnectionString,
                    serverInfo.Server.ItvName,
                    serverInfo.AssignedRoles.Count);
                if (serverInfo.Server.Id == -1)
                {
                    db.Servers.Add(new Server
                    {
                        Ip = serverInfo.Server.Ip,
                        Mode = serverInfo.Server.Mode,
                        Description = serverInfo.Server.Description,
                        ConnectionString = serverInfo.Server.ConnectionString,
                        ItvName = serverInfo.Server.ItvName
                    });
                }
                else
                {
                    db.Servers.Update(serverInfo.Server);
                }
                db.SaveChanges();
                Server server = db.Servers.First(s => (s.Description == serverInfo.Server.Description
                        && s.Ip == serverInfo.Server.Ip
                        && s.Mode == serverInfo.Server.Mode));
                IEnumerable<ServersRoles> serversRoles = db.ServersRoles.ToList().Where(w => w.ServerId == server.Id);
                foreach (ServersRoles relationship in serversRoles)
                {
                    db.ServersRoles.Remove(relationship);
                }
                db.SaveChanges();

                foreach (string roleId in serverInfo.AssignedRoles)
                {
                    if (!(string.IsNullOrEmpty(roleId) || string.IsNullOrWhiteSpace(roleId)))
                    {
                        Console.WriteLine("Adding {0} to {1}", server.Description, db.Roles.First(r => r.Id == roleId).Name);
                        db.ServersRoles.Add(new ServersRoles { ServerId = server.Id, Role = await RoleManager.FindByIdAsync(roleId) });
                    }
                }
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Console.WriteLine(ex.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
                else
                {
                    Console.WriteLine(ex.InnerException.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.InnerException.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
            }
            return Json(new JsonModel { Result = true });
        }

        [HttpPost]
        public IActionResult ServerDelete(int id)
        {
            List<IdentityError> errors = new List<IdentityError>();
            //Console.WriteLine("Delete role {0}", id);
            try
            {
                Server server = db.Servers.First(w => w.Id == id);
                db.Servers.Remove(server);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Console.WriteLine(ex.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
                else
                {
                    Console.WriteLine(ex.InnerException.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.InnerException.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
            }
            return Json(new JsonModel { Result = true });


        }

        #endregion

        #region EntryManager
        [HttpGet]
        public IActionResult EntryEdit(string id)
        {
            //Console.WriteLine("Edit Server");
            return ViewComponent("EntryEdit", id);
        }

        [HttpPost]
        public IActionResult EntryEdit(Entry entry)
        {
            List<IdentityError> errors = new List<IdentityError>();
            try
            {
                Server server = db.Servers.First(s => s.Id == entry.ServerId);
                entry.Server = server;

                Console.WriteLine(entry.Id);
                Console.WriteLine("Description:{0}\r\nServer:{1}\r\nRayType:{2}\r\nNoE:{3}\r\nEnterRay:{4}\r\nExitRay:{5}", entry.Description,
                            entry.Server.Description,
                            entry.RaysType,
                            entry.NoE,
                            entry.EnterRay,
                            entry.ExitRay);
                Console.WriteLine(entry.Server.Id);

                if (entry.Id == -1)
                {
                    Console.WriteLine("New Server");
                    db.Entries.Add(entry);
                }
                else
                {
                    db.Entries.Update(entry);
                }
                db.SaveChanges();
                Console.WriteLine(db.Entries.FirstOrDefault().Server.Description);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Console.WriteLine(ex.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
                else
                {
                    Console.WriteLine(ex.InnerException.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.InnerException.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
            }
            return Json(new JsonModel { Result = true });
        }

        [HttpPost]
        public IActionResult EntryDelete(int id)
        {
            List<IdentityError> errors = new List<IdentityError>();
            //Console.WriteLine("Delete role {0}", id);
            try
            {
                Entry entry = db.Entries.First(e => e.Id == id);
                db.Entries.Remove(entry);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Console.WriteLine(ex.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
                else
                {
                    Console.WriteLine(ex.InnerException.Message);
                    errors.Add(new IdentityError { Code = "8086", Description = ex.InnerException.Message });
                    return Json(new JsonModel { Result = false, Errors = errors.AsEnumerable() });
                }
            }
            return Json(new JsonModel { Result = true });


        }

        #endregion

    }
}
