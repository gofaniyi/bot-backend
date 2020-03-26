using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloEpidBot.Migrations
{
    public partial class AddReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symptoms = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    RiskStatus = table.Column<string>(nullable: false),
                    ReporterName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    DateReported = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "Age", "DateReported", "Location", "ReporterName", "RiskStatus", "Symptoms" },
                values: new object[,]
                {
                    { 100, 25, new DateTime(2020, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cooperative Villas, Ajah", "Kenny", "High", "I have been experiencing sever cough and headache" },
                    { 101, 50, new DateTime(2020, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cooperative Villas, Ajah", "Teju", "High", "cough and difficulty in breathing" },
                    { 102, 70, new DateTime(2020, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cooperative Villas, Ajah", "Kenny", "low", "None" },
                    { 103, 30, new DateTime(2020, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cooperative Villas, Ajah", "Kenny", "low", "None" },
                    { 4, 80, new DateTime(2020, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cooperative Villas, Ajah", "Tega", "High", "sever cough and headache" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
