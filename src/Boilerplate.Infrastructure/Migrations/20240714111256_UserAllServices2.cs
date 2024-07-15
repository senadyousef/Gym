using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boilerplate.Infrastructure.Migrations
{
    public partial class UserAllServices2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$6RUnycZMz47EO254rwac4.UJqMgk0FBHvCQKr9I0Rtsg/ZwNBKUzq");

            migrationBuilder.CreateIndex(
                name: "IX_UserAllServices_AllServicesId",
                table: "UserAllServices",
                column: "AllServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAllServices_UserId",
                table: "UserAllServices",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAllServices_AllServices_AllServicesId",
                table: "UserAllServices",
                column: "AllServicesId",
                principalTable: "AllServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAllServices_Users_UserId",
                table: "UserAllServices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAllServices_AllServices_AllServicesId",
                table: "UserAllServices");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAllServices_Users_UserId",
                table: "UserAllServices");

            migrationBuilder.DropIndex(
                name: "IX_UserAllServices_AllServicesId",
                table: "UserAllServices");

            migrationBuilder.DropIndex(
                name: "IX_UserAllServices_UserId",
                table: "UserAllServices");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Xz7kO23k6knXGtCFYoheCeKXPjPxOjJS1iuq7/MsWQ9ynriA3.doW");
        }
    }
}
