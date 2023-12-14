using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFindersAPI.Migrations.TestDatabaseMigrations
{
    /// <inheritdoc />
    public partial class AddingUserLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    DestinationId = table.Column<string>(type: "TEXT", nullable: false),
                    XCoordinate = table.Column<double>(type: "REAL", nullable: false),
                    YCoordinate = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__locations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_locations");
        }
    }
}
