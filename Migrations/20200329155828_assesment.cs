using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloEpidBot.Migrations
{
    public partial class assesment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetAssesments_questions_questionsId",
                table: "GetAssesments");

            migrationBuilder.DropIndex(
                name: "IX_GetAssesments_questionsId",
                table: "GetAssesments");

            migrationBuilder.DropColumn(
                name: "questionsId",
                table: "GetAssesments");

            migrationBuilder.AddColumn<string>(
                name: "assesmentId",
                table: "questions",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 16, 58, 27, 716, DateTimeKind.Local).AddTicks(1396));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 100,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 16, 58, 27, 713, DateTimeKind.Local).AddTicks(160));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 101,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 16, 58, 27, 716, DateTimeKind.Local).AddTicks(1315));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 102,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 16, 58, 27, 716, DateTimeKind.Local).AddTicks(1389));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 103,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 16, 58, 27, 716, DateTimeKind.Local).AddTicks(1393));

            migrationBuilder.CreateIndex(
                name: "IX_questions_assesmentId",
                table: "questions",
                column: "assesmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_questions_GetAssesments_assesmentId",
                table: "questions",
                column: "assesmentId",
                principalTable: "GetAssesments",
                principalColumn: "assesmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_GetAssesments_assesmentId",
                table: "questions");

            migrationBuilder.DropIndex(
                name: "IX_questions_assesmentId",
                table: "questions");

            migrationBuilder.DropColumn(
                name: "assesmentId",
                table: "questions");

            migrationBuilder.AddColumn<string>(
                name: "questionsId",
                table: "GetAssesments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 15, 23, 35, 419, DateTimeKind.Local).AddTicks(4584));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 100,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 15, 23, 35, 416, DateTimeKind.Local).AddTicks(5447));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 101,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 15, 23, 35, 419, DateTimeKind.Local).AddTicks(4506));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 102,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 15, 23, 35, 419, DateTimeKind.Local).AddTicks(4576));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 103,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 15, 23, 35, 419, DateTimeKind.Local).AddTicks(4580));

            migrationBuilder.CreateIndex(
                name: "IX_GetAssesments_questionsId",
                table: "GetAssesments",
                column: "questionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GetAssesments_questions_questionsId",
                table: "GetAssesments",
                column: "questionsId",
                principalTable: "questions",
                principalColumn: "questionsId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
