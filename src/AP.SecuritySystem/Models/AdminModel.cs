using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AP.SecuritySystem.Models
{
    public class AdminVCModel
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public int ServerType { get; set; }
    }
    public class AdminUserVCModel
    {
        public IQueryable<UserModel> Users { get; set; }
        public AdminVCModel Header { get; set; }
    }
    public class AdminServerVCModel
    {
        public IQueryable<Server> Servers { get; set; }
        public AdminVCModel Header { get; set; }
    }

    public class JsonModel
    {
        public bool Result { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }

    public class UserModel
    {
        public ApplicationUser User { get; set; }
        public IQueryable<IdentityRole> AssignedRoles { get; set; }
        public IQueryable<IdentityRole> AvailableRoles { get; set; }
    }

    public class ServerModel
    {
        public Server Server { get; set; }
        public IQueryable<IdentityRole> AssignedRoles { get; set; }
        public IQueryable<IdentityRole> AvailableRoles { get; set; }
    }

    public class EntryModel
    {
        public Entry Entry { get; set; }
        public List<Server> Servers { get; set; }
    }

    public class RoleModel
    {
        public IdentityRole Role { get; set; }
        public IQueryable<ApplicationUser> AssignedUsers { get; set; }
        public IQueryable<Server> AssignedServers { get; set; }
        public IQueryable<ApplicationUser> AvailableUsers { get; set; }
        public IQueryable<Server> AvailableServers { get; set; }

    }

    public class ResetPasswordModel
    {
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
    }

    public class EditUserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> AssignedRoles { get; set; }
    }

    public class EditRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> AssignedUsers { get; set; }
        public List<int> AssignedServers { get; set; }
    }

    public class EditServerRole
    {
        public Server Server { get; set; }
        public List<string> AssignedRoles { get; set; }
    }


    public class AdminRoleVCModel
    {
        public IQueryable<RoleModel> Roles { get; set; }
        public AdminVCModel Header { get; set; }
    }

    public class AdminModel
    {
        public int videoServersCount { get; set; }
        public int backupServersCount { get; set; }
        public int analyticsServersCount { get; set; }
        public int usersCount { get; set; }
        public int roleCount { get; set; }
    }

    public class AddUserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> AssignedRoles { get; set; }

    }

}
