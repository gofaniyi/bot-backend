using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloEpidBot.Migrations
{
    public partial class assesment1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "publicKey",
                table: "GetAssesments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PartnerTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    PartnerId = table.Column<string>(nullable: true),
                    PartnerName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DateJoined = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 19, 4, 37, 118, DateTimeKind.Local).AddTicks(4628));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 100,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 19, 4, 37, 116, DateTimeKind.Local).AddTicks(7145));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 101,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 19, 4, 37, 118, DateTimeKind.Local).AddTicks(4570));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 102,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 19, 4, 37, 118, DateTimeKind.Local).AddTicks(4622));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 103,
                column: "DateReported",
                value: new DateTime(2020, 3, 29, 19, 4, 37, 118, DateTimeKind.Local).AddTicks(4625));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartnerTokens");

            migrationBuilder.DropColumn(
                name: "publicKey",
                table: "GetAssesments");

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
        }
    }
}
