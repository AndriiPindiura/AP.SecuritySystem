using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AP.SecuritySystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.SecuritySystem.Controllers.ViewComponents
{
    [Authorize]
    public class RoleListViewComponent : ViewComponent
    {
        public RoleListViewComponent(ApplicationDbContext database)
        {
            db = database;
        }
        private readonly ApplicationDbContext db;

        private IQueryable<RoleModel> GetRoles()
        {
            List<RoleModel> roleList = new List<RoleModel>();
            //List<UserModel> users = new List<UserModel>();
            foreach (var role in db.Roles.ToList())
            {
                //Console.WriteLine(role.Name);
                //var roles = db.Roles.Where(u => u.Users.Any(x => x.UserId == user.Id)).ToList();

                try
                {
                    //var cores = db.CoreRoles.Select(c => c.Cores).Where(r => r.Roles.Any(x => x.Id == role.Id));
                    //var cores2 = db.CoreRoles.Select(s => s.Cores);
                    //var cores = db.CoreRoles.Where(r => r.Roles.Id == role.Id).Select(s => s.Cores);
                    //Console.WriteLine(cores.ToList().Count);
                    //cores.Where(x => x.Roles.Any(r => r.RoleId == role.Id));
                    roleList.Add(new RoleModel
                    {
                        Role = role,
                        AssignedUsers = db.Users
                            .Where(r => r.Roles.Any(x => x.RoleId == role.Id)),
                        AssignedServers = db.ServersRoles
                            .Where(r => r.Role.Id == role.Id)
                            .Select(s => s.Server)

                    });
                    //Console.WriteLine(roleList.First().Cores.First());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return roleList.AsQueryable();
        }


        // GET: /<controller>/
        [Authorize(Roles = "admin")]
        public async Task<IViewComponentResult> InvokeAsync(AdminVCModel data)
        {
            AdminRoleVCModel view = new AdminRoleVCModel();
            view.Header = new AdminVCModel { Title = data.Title, Icon = data.Icon, ServerType = data.ServerType };
            view.Roles = await Task.FromResult(GetRoles());
            Console.WriteLine(data.Title);
            //var items = await Task.FromResult(GetUsers());
            return View("RoleList", view);
            /*foreach (ApplicationUser user in db.Users)
            {
                user.UserName
            }*/
        }
    }
}
