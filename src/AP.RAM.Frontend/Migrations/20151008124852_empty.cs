using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace AP.RMA.Frontend.Migrations
{
    public partial class empty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cores",
                columns: table => new
                {
                    Cid = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Address = table.Column<string>(isNullable: true),
                    Description = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreModel", x => x.Cid);
                });
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Uid = table.Column<string>(isNullable: false),
                    Hash = table.Column<string>(isNullable: false),
                    IsAdmin = table.Column<bool>(isNullable: false),
                    Salt = table.Column<string>(isNullable: false),
                    Username = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginModel", x => x.Uid);
                });
            migrationBuilder.CreateTable(
                name: "Rights",
                columns: table => new
                {
                    Rid = table.Column<string>(isNullable: false),
                    Cid = table.Column<int>(isNullable: false),
                    Uid = table.Column<string>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RightsModel", x => x.Rid);
                });
            migrationBuilder.CreateIndex(
                name: "IX_LoginModel_Username",
                table: "Users",
                column: "Username",
                isUnique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Cores");
            migrationBuilder.DropTable("Users");
            migrationBuilder.DropTable("Rights");
        }
    }
}
