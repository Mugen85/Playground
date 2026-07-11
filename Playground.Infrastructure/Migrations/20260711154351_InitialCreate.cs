using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Playground.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    SuggestedDonation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsUrgent = table.Column<bool>(type: "INTEGER", nullable: false),
                    PetType = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pets");
        }
    }
}
