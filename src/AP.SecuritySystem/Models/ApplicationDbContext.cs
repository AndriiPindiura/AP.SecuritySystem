using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.SecuritySystem.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Server> Servers { get; set; }
        //public DbSet<Role> APRoles { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<ServersRoles> ServersRoles { get; set; }
        public DbSet<Protocol> Protocol { get; set; }
        public DbSet<ProtocolCRM> ProtocolCRM { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Server>().ToTable("AP.Servers");
            modelBuilder.Entity<Entry>().ToTable("AP.Entries");
            modelBuilder.Entity<ServersRoles>().ToTable("AP.ServersRoles");
            modelBuilder.Entity<Protocol>().ToTable("AP.Protocol");
            modelBuilder.Entity<ProtocolCRM>().ToTable("AP.ProtocolCRM");

            //modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("ap.asp.UserLogins");
            //modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("ap.asp.RoleClaims");
            //modelBuilder.Entity<IdentityUserRole<string>>()
            //.HasKey(r => new { r.UserId, r.RoleId });
            //.ToTable("AspNetUserRoles");

            //modelBuilder.Entity<IdentityUserLogin<string>>()
            //    .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
            //.ToTable("AspNetUserLogins");
            /* 
             modelBuilder.Entity<Core>().Key(k => k.CoreId);
             modelBuilder.Entity<AnalyticsRole>().Key(k => k.Id );
             modelBuilder.Entity<Entry>().Key(k => k.Id);
             modelBuilder.Entity<Protocol>().Key(k => k.Id);
             modelBuilder.Entity<ProtocolCRM>().Key(k => k.Id);
             modelBuilder.Entity<Core>().ToTable("Cores");
             modelBuilder.Entity<AnalyticsRole>().ToTable("UserCores");
             modelBuilder.Entity<Entry>().ToTable("Entries");
             */
            /*modelBuilder.Entity<LoginModel>().Key(k => k.Uid);
            modelBuilder.Entity<LoginModel>().Property(m => m.Login).MaxLength(50);
            modelBuilder.Entity<LoginModel>().Index(i => i.Login).Unique();
            modelBuilder.Entity<LoginModel>().Property(p => p.Hash).Required();
            modelBuilder.Entity<LoginModel>().Property(p => p.Salt).Required();
            modelBuilder.Entity<RightsModel>().Key(k => k.Rid);
            modelBuilder.Entity<RightsModel>().Property(p => p.Cid).Required();
            modelBuilder.Entity<RightsModel>().Property(p => p.Uid).Required();
            modelBuilder.Entity<CoreModel>().Key(k => k.Cid);
            modelBuilder.Entity<CoreModel>().Property(p => p.Cid).Required();
            modelBuilder.Entity<LoginModel>().ToTable("Users");
            modelBuilder.Entity<CoreModel>().ToTable("Cores");
            modelBuilder.Entity<RightsModel>().ToTable("Rights");*/

        }

    }

    public class ApplicationUser : IdentityUser
    {
    }
}
