using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using AP.SecuritySystem.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AP.SecuritySystem.Controllers.ViewComponents
{
    [Authorize]
    public class ServerEditViewComponent : ViewComponent
    {
        public ServerEditViewComponent(ApplicationDbContext database)
        {
            db = database;
        }

        private readonly ApplicationDbContext db;

        private ServerModel GetServerInfo(int serverId)
        {
            try
            {
                var roles = db.Roles.ToList();
                //Console.WriteLine(userId);
                var assignedRoles = db.ServersRoles.Where(w => w.ServerId == serverId).Select(s => s.Role).ToList();
                var availableRoles = roles.Except(assignedRoles);
                //Console.WriteLine("assigned: {0}\r\navailable: {1}", assignedRoles.ToList().Count, availableRoles.ToList().Count);
                return new ServerModel
                {
                    Server = db.Servers.FirstOrDefault(i => i.Id == serverId),
                    AssignedRoles = assignedRoles.AsQueryable(),
                    AvailableRoles = availableRoles.AsQueryable()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;/* new UserModel
            {
                User = db.Users.First(i => i.Id == userId),
                AssignedRoles = assignedRoles.AsQueryable(),
                AvailableRoles = db.Roles.Except(assignedRoles.AsEnumerable())
            };*/
                        //.Where(u => u.Users.Any(x => x.UserId != userId))};
        }


        // GET: /<controller>/
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            Console.WriteLine("Server Edit View Component with " + id);
            ServerModel serverInfo = new ServerModel();
            int serverId = -1;
            Int32.TryParse(id, out serverId);
            serverInfo = await Task.FromResult(GetServerInfo(serverId));
            return View("ServerEdit", serverInfo);
        }
    }
}
