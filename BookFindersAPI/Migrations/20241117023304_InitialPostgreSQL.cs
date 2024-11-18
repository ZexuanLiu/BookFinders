using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookFindersAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    BookId = table.Column<string>(type: "text", nullable: false),
                    ThumbsUp = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PostDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_coordinates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false),
                    Z = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__coordinates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DestinationId = table.Column<string>(type: "text", nullable: false),
                    XCoordinate = table.Column<double>(type: "double precision", nullable: false),
                    YCoordinate = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_pushNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__pushNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_userTrackingSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Campus = table.Column<string>(type: "text", nullable: false),
                    TimeStarted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeEnded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__userTrackingSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_userTrackingInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoordinateId = table.Column<int>(type: "integer", nullable: false),
                    PostDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserTrackingSessionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__userTrackingInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK__userTrackingInstances__coordinates_CoordinateId",
                        column: x => x.CoordinateId,
                        principalTable: "_coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__userTrackingInstances__userTrackingSessions_UserTrackingSe~",
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
                name: "_comment");

            migrationBuilder.DropTable(
                name: "_locations");

            migrationBuilder.DropTable(
                name: "_pushNotifications");

            migrationBuilder.DropTable(
                name: "_userTrackingInstances");

            migrationBuilder.DropTable(
                name: "_coordinates");

            migrationBuilder.DropTable(
                name: "_userTrackingSessions");
        }
    }
}
