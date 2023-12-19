using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boilerplate.Infrastructure.Migrations
{
    public partial class Trainee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalTrainersClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalTrainer = table.Column<int>(type: "int", nullable: false),
                    Trainee = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false), 
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
                    table.PrimaryKey("PK_PersonalTrainersClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalTrainersClasses_Users_PersonalTrainer",
                        column: x => x.PersonalTrainer,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalTrainersClasses_Users_Trainee",
                        column: x => x.Trainee,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Zs8NqgwrRBpRsg9pLQ1wh.PTfXJGviGONVDht6wUqpdP78GVUVSeS");

            migrationBuilder.CreateIndex(
                name: "FK_PersonalTrainersClasses_Users_PersonalTrainer",
                table: "PersonalTrainersClasses",
                column: "PersonalTrainer");

            migrationBuilder.CreateIndex(
                name: "FK_PersonalTrainersClasses_Users_Trainee",
                table: "PersonalTrainersClasses",
                column: "Trainee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalTrainersClasses");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$8FefSnew3fy1Mxd7mnlQwuOQ7uzSzHXYywk4agbVGfO02qcYABD..");
        }
    }
}
