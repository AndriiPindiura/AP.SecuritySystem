using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace AP.RMA.Frontend.Migrations
{
    public partial class add_fullname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_LoginModel_Username", table: "Users");
            migrationBuilder.DropColumn(name: "Username", table: "Users");
            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "Users",
                isNullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Users",
                isNullable: true);
            migrationBuilder.CreateIndex(
                name: "IX_LoginModel_Login",
                table: "Users",
                column: "Login",
                isUnique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_LoginModel_Login", table: "Users");
            migrationBuilder.DropColumn(name: "Fullname", table: "Users");
            migrationBuilder.DropColumn(name: "Login", table: "Users");
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                isNullable: true);
            migrationBuilder.CreateIndex(
                name: "IX_LoginModel_Username",
                table: "Users",
                column: "Username",
                isUnique: true);
        }
    }
}
