using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using AP.SecuritySystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.SecuritySystem.Controllers.ViewComponents
{
    [Authorize]
    public class RoleEditViewComponent : ViewComponent
    {
        public RoleEditViewComponent(ApplicationDbContext database)
        {
            db = database;
        }

        private readonly ApplicationDbContext db;

        private RoleModel GetRoleInfo(string roleId)
        {
            try
            {

                var users = db.Users.ToList();
                var servers = db.Servers.ToList();
                var entries = db.Entries.ToList();
                //Console.WriteLine(userId);
                var assignedUsers = db.Users.Where(r => r.Roles.Any(a => a.RoleId == roleId)).ToList();
                var availableUsers = users.Except(assignedUsers);
                var assignedServers = db.Servers.Where(r => r.Roles.Any(a => a.Role.Id == roleId)).ToList();
                var availableServers = servers.Except(assignedServers);

                //Console.WriteLine("assigned: {0}\r\navailable: {1}", assignedRoles.ToList().Count, availableRoles.ToList().Count);
                return new RoleModel
                {
                    Role = db.Roles.FirstOrDefault(i => i.Id == roleId),
                    AssignedUsers = assignedUsers.AsQueryable(),
                    AvailableUsers = availableUsers.AsQueryable(),
                    AssignedServers = assignedServers.AsQueryable(),
                    AvailableServers = availableServers.AsQueryable(),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;

        }

        // GET: /<controller>/
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            RoleModel roleInfo = new RoleModel();
            roleInfo = await Task.FromResult(GetRoleInfo(id));

            //Console.WriteLine("User Edit View Component with " + id);
            return View("RoleEdit", roleInfo);
        }
    }
}
