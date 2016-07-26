using AP.SecuritySystem.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using System;

namespace AP.SecuritySystem.Controllers
{
    [Authorize]
    public class ServerListViewComponent : ViewComponent
    {
        public ServerListViewComponent(ApplicationDbContext database)
        {
            db = database;
        }

        private readonly ApplicationDbContext db;

        private IQueryable<Server> GetServers(int serverType)
        {
            
            return db.Servers.Where(x => x.Mode == serverType).Include(e => e.Entries).Include(r => r.Roles);
        }

        [Authorize(Roles = "admin")]
        public async Task<IViewComponentResult> InvokeAsync(AdminVCModel data)
        {
            AdminServerVCModel view = new AdminServerVCModel();
            view.Header = new AdminVCModel { Title = data.Title, Icon = data.Icon, ServerType = data.ServerType };
            view.Servers = await Task.FromResult(GetServers(data.ServerType));
            try
            {

                foreach (var server in view.Servers)
                {
                    foreach (var role in server.Roles)
                    {
                        Console.WriteLine("{0} - {1}", server.Description, role.Role.Name);
                    }
                }
                return View("ServerList", view);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("ServerList");
            }
        }
    }

}
