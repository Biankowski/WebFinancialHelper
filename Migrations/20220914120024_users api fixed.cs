using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFinancialHelper.Migrations
{
    public partial class usersapifixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectedData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponsibleUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfPurchase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PurchaseDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectedData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectedData");
        }
    }
}
