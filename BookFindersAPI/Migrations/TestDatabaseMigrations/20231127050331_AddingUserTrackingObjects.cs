using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFindersAPI.Migrations.TestDatabaseMigrations
{
    /// <inheritdoc />
    public partial class AddingUserTrackingObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_userTrackingSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Campus = table.Column<string>(type: "TEXT", nullable: true),
                    TimeStarted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TimeEnded = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__userTrackingSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coordinate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<float>(type: "REAL", nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_userTrackingInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoordinateId = table.Column<int>(type: "INTEGER", nullable: true),
                    PostDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserTrackingSessionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__userTrackingInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK__userTrackingInstances_Coordinate_CoordinateId",
                        column: x => x.CoordinateId,
                        principalTable: "Coordinate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__userTrackingInstances__userTrackingSessions_UserTrackingSessionId",
                        column: x => x.UserTrackingSessionId,
                        principalTable: "_userTrackingSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX__userTrackingInstances_CoordinateId",
                table: "_userTrackingInstances",
                column: "CoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX__userTrackingInstances_UserTrackingSessionId",
                table: "_userTrackingInstances",
                column: "UserTrackingSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_userTrackingInstances");

            migrationBuilder.DropTable(
                name: "Coordinate");

            migrationBuilder.DropTable(
                name: "_userTrackingSessions");
        }
    }
}
