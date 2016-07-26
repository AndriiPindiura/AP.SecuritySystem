using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using AP.RMA.Frontend.CA.Models;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace AP.RMA.Frontend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class add_fullname
    {
        public override string Id
        {
            get { return "20151008182327_add_fullname"; }
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn);

            modelBuilder.Entity("AP.RMA.Frontend.Models.CoreModel", b =>
                {
                    b.Property<int>("Cid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Description");

                    b.Key("Cid");

                    b.Annotation("Relational:TableName", "Cores");
                });

            modelBuilder.Entity("AP.RMA.Frontend.Models.LoginModel", b =>
                {
                    b.Property<string>("Uid");

                    b.Property<string>("Fullname");

                    b.Property<string>("Hash")
                        .Required();

                    b.Property<bool>("IsAdmin");

                    b.Property<string>("Login")
                        .Annotation("MaxLength", 50);

                    b.Property<string>("Salt")
                        .Required();

                    b.Key("Uid");

                    b.Index("Login")
                        .Unique();

                    b.Annotation("Relational:TableName", "Users");
                });

            modelBuilder.Entity("AP.RMA.Frontend.Models.RightsModel", b =>
                {
                    b.Property<string>("Rid");

                    b.Property<int>("Cid");

                    b.Property<string>("Uid")
                        .Required();

                    b.Key("Rid");

                    b.Annotation("Relational:TableName", "Rights");
                });
        }
    }
}
