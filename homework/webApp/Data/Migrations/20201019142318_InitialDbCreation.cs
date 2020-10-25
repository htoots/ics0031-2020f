using Microsoft.EntityFrameworkCore.Migrations;

namespace webApp.Data.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiffieHellmanResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SecretA = table.Column<ulong>(nullable: false),
                    SecretB = table.Column<ulong>(nullable: false),
                    ModulusP = table.Column<ulong>(nullable: false),
                    BaseG = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiffieHellmanResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RSAResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PrimeP = table.Column<ulong>(nullable: false),
                    PrimeQ = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSAResults", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiffieHellmanResults");

            migrationBuilder.DropTable(
                name: "RSAResults");
        }
    }
}
