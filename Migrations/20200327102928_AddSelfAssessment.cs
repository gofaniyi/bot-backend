using Microsoft.EntityFrameworkCore.Migrations;

namespace GloEpidBot.Migrations
{
    public partial class AddSelfAssessment : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assesments");
        }
    }
}
