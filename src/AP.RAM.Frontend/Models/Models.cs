using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.RMA.Frontend.CA.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<LoginModel> Users { get; set; }
        public DbSet<CoreModel> Cores { get; set; }
        public DbSet<RightsModel> Rights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginModel>().Key(k => k.Uid);
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
            modelBuilder.Entity<RightsModel>().ToTable("Rights");

        }

    }


}
