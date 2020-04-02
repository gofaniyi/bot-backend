using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloEpidBot.Migrations
{
    public partial class inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assesments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TravelHistory = table.Column<string>(nullable: true),
                    PublicPlace = table.Column<string>(nullable: true),
                    PublicPlaces = table.Column<string>(nullable: true),
                    TravelPlaces = table.Column<string>(nullable: true),
                    CloseContact = table.Column<string>(nullable: true),
                    Ocupation = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    HouseAddress = table.Column<string>(nullable: true),
                    Symptoms = table.Column<string>(nullable: true),
                    SymptomsStart = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assesments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    questionsId = table.Column<string>(nullable: false),
                    question = table.Column<string>(nullable: true),
                    response = table.Column<string>(nullable: true),
                    score = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.questionsId);
                });

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

            migrationBuilder.CreateTable(
                name: "GetAssesments",
                columns: table => new
                {
                    assesmentId = table.Column<string>(nullable: false),
                    source = table.Column<string>(nullable: true),
                    questionsId = table.Column<string>(nullable: true),
                    evaluationScore = table.Column<string>(nullable: true),
                    evaluationTime = table.Column<DateTime>(nullable: false),
                    evaluationOutcome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetAssesments", x => x.assesmentId);
                    table.ForeignKey(
                        name: "FK_GetAssesments_questions_questionsId",
                        column: x => x.questionsId,
                        principalTable: "questions",
                        principalColumn: "questionsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "Age", "DateReported", "Location", "ReporterName", "RiskStatus", "Symptoms" },
                values: new object[,]
                {
                    { 100, 25, new DateTime(2020, 3, 29, 15, 23, 35, 416, DateTimeKind.Local).AddTicks(5447), "Cooperative Villas, Ajah", "Kenny", "High", "I have been experiencing sever cough and headache" },
                    { 101, 50, new DateTime(2020, 3, 29, 15, 23, 35, 419, DateTimeKind.Local).AddTicks(4506), "Cooperative Villas, Ajah", "Teju", "High", "cough and difficulty in breathing" },
                    { 102, 70, new DateTime(2020, 3, 29, 15, 23, 35, 419, DateTimeKind.Local).AddTicks(4576), "Cooperative Villas, Ajah", "Kenny", "low", "None" },
                    { 103, 30, new DateTime(2020, 3, 29, 15, 23, 35, 419, DateTimeKind.Local).AddTicks(4580), "Cooperative Villas, Ajah", "Kenny", "low", "None" },
                    { 4, 80, new DateTime(2020, 3, 29, 15, 23, 35, 419, DateTimeKind.Local).AddTicks(4584), "Cooperative Villas, Ajah", "Tega", "High", "sever cough and headache" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GetAssesments_questionsId",
                table: "GetAssesments",
                column: "questionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assesments");

            migrationBuilder.DropTable(
                name: "GetAssesments");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "questions");
        }
    }
}
