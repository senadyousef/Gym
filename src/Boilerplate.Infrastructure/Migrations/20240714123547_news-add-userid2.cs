using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boilerplate.Infrastructure.Migrations
{
    public partial class newsadduserid2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
               name: "IX_News_UserId",
               table: "News",
               column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Users_UserId",
                table: "News",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$vTw/guMM809yHWK0syCts.7Ho27uOAr3BC9Wmnr0CS/7CalhXKO.u");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$x82/Fd6LF6oTUuQyIDXvCO5qqqYEO9XUUlvSCrETjKEti4tnSyeQ2");
        }
    }
}
