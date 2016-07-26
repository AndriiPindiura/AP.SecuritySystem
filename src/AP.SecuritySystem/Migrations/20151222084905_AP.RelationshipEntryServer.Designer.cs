using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using AP.SecuritySystem.Models;

namespace ap.SecuritySystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20151222084905_AP.RelationshipEntryServer")]
    partial class APRelationshipEntryServer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AP.SecuritySystem.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.Entry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("EnterCamera");

                    b.Property<int>("EnterDelay");

                    b.Property<int>("EnterRay");

                    b.Property<int>("ExitCamera");

                    b.Property<int>("ExitDelay");

                    b.Property<int>("ExitRay");

                    b.Property<int>("NoE");

                    b.Property<bool>("RaysType");

                    b.Property<int?>("ServerId");

                    b.Property<int>("UpCamera");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AP.Entries");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.Protocol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<DateTime>("Date");

                    b.Property<Guid>("Guid");

                    b.Property<int>("ObjId");

                    b.Property<int>("ServerId");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AP.Protocol");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.ProtocolCRM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CMRId");

                    b.Property<string>("CRMState");

                    b.Property<string>("Culture");

                    b.Property<DateTime>("Date");

                    b.Property<Guid>("Guid");

                    b.Property<string>("LicensePlate");

                    b.Property<int>("NoE");

                    b.Property<int>("ServerId");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AP.ProtocolCRM");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.Server", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConnectionString");

                    b.Property<string>("Description");

                    b.Property<string>("Ip");

                    b.Property<string>("ItvName");

                    b.Property<int>("Mode");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AP.Servers");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.ServersRoles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoleId");

                    b.Property<int>("ServerId");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AP.ServersRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.Entry", b =>
                {
                    b.HasOne("AP.SecuritySystem.Models.Server")
                        .WithMany()
                        .HasForeignKey("ServerId");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.Protocol", b =>
                {
                    b.HasOne("AP.SecuritySystem.Models.Server")
                        .WithMany()
                        .HasForeignKey("ServerId");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.ProtocolCRM", b =>
                {
                    b.HasOne("AP.SecuritySystem.Models.Server")
                        .WithMany()
                        .HasForeignKey("ServerId");
                });

            modelBuilder.Entity("AP.SecuritySystem.Models.ServersRoles", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("AP.SecuritySystem.Models.Server")
                        .WithMany()
                        .HasForeignKey("ServerId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AP.SecuritySystem.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AP.SecuritySystem.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("AP.SecuritySystem.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
