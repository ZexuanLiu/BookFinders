using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFindersAPI.Migrations.TestDatabaseMigrations
{
    /// <inheritdoc />
    public partial class FixingSomeUserTrackingVariables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__userTrackingInstances_Coordinate_CoordinateId",
                table: "_userTrackingInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coordinate",
                table: "Coordinate");

            migrationBuilder.RenameTable(
                name: "Coordinate",
                newName: "_coordinates");

            migrationBuilder.AddPrimaryKey(
                name: "PK__coordinates",
                table: "_coordinates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__userTrackingInstances__coordinates_CoordinateId",
                table: "_userTrackingInstances",
                column: "CoordinateId",
                principalTable: "_coordinates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__userTrackingInstances__coordinates_CoordinateId",
                table: "_userTrackingInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK__coordinates",
                table: "_coordinates");

            migrationBuilder.RenameTable(
                name: "_coordinates",
                newName: "Coordinate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coordinate",
                table: "Coordinate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__userTrackingInstances_Coordinate_CoordinateId",
                table: "_userTrackingInstances",
                column: "CoordinateId",
                principalTable: "Coordinate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
