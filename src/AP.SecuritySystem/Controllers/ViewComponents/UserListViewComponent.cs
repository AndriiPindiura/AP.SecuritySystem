using Microsoft.AspNet.Mvc;
using System.Linq;
using AP.SecuritySystem.Models;
using Microsoft.AspNet.Authorization;
using System.Threading.Tasks;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace AP.SecuritySystem.Controllers
{
    [Authorize]
    public class UserListViewComponent : ViewComponent
    {
        public UserListViewComponent(ApplicationDbContext database)
        {
            db = database;

        }
        private readonly ApplicationDbContext db;

        private IQueryable<UserModel> GetUsers()
        {
            List<UserModel> userList = new List<UserModel>();
            //List<UserModel> users = new List<UserModel>();
            foreach (var user in db.Users.ToList())
            {
                //Console.WriteLine(user.UserName);
                //var roles = db.Roles.Where(u => u.Users.Any(x => x.UserId == user.Id)).ToList();
                try
                {
                    userList.Add(new UserModel { User = user, AssignedRoles = db.Roles.Where(u => u.Users.Any(x => x.UserId == user.Id)) });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return userList.AsQueryable();
        }

        private IQueryable<IdentityRole> GetRoles(string id)
        {
            return db.Roles.Where(u => u.Users.Select(r => r.RoleId).Contains(id));

        }

        [Authorize(Roles = "admin")]
        public async Task<IViewComponentResult> InvokeAsync(AdminVCModel data)
        {
            AdminUserVCModel view = new AdminUserVCModel();
            view.Header = new AdminVCModel { Title = data.Title, Icon = data.Icon, ServerType = data.ServerType };
            view.Users = await Task.FromResult(GetUsers());
            //Console.WriteLine(data.Title);
            //var items = await Task.FromResult(GetUsers());
            return View("UserList", view);
        }


    }
}
