using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boilerplate.Infrastructure.Migrations
{
    public partial class Children_add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    BOD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MembershipStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembershipExpDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushTokenId = table.Column<int>(type: "int", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhotoUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisabledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisabledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Children_PushToken_PushTokenId",
                        column: x => x.PushTokenId,
                        principalTable: "PushToken",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Children_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$whzt.ifKbfXKKWgWzojm/uipabYoZSFhGf/CDJ.cObbLdtfYKr.ZO");

            migrationBuilder.CreateIndex(
                name: "IX_Children_PushTokenId",
                table: "Children",
                column: "PushTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Children_UserId",
                table: "Children",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Children");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Zs8NqgwrRBpRsg9pLQ1wh.PTfXJGviGONVDht6wUqpdP78GVUVSeS");
        }
    }
}
