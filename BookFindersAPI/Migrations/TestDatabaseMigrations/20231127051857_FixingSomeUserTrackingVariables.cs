using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFindersAPI.Migrations.TestDatabaseMigrations
{
    /// <inheritdoc />
    public partial class FixingSomeUserTrackingVariables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__userTrackingInstances_Coordinate_CoordinateId",
                table: "_userTrackingInstances");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Coordinate",
                newName: "Z");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Coordinate",
                newName: "Y");

            migrationBuilder.AddColumn<float>(
                name: "X",
                table: "Coordinate",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStarted",
                table: "_userTrackingSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeEnded",
                table: "_userTrackingSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Campus",
                table: "_userTrackingSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDateTime",
                table: "_userTrackingInstances",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CoordinateId",
                table: "_userTrackingInstances",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__userTrackingInstances_Coordinate_CoordinateId",
                table: "_userTrackingInstances",
                column: "CoordinateId",
                principalTable: "Coordinate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__userTrackingInstances_Coordinate_CoordinateId",
                table: "_userTrackingInstances");

            migrationBuilder.DropColumn(
                name: "X",
                table: "Coordinate");

            migrationBuilder.RenameColumn(
                name: "Z",
                table: "Coordinate",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Y",
                table: "Coordinate",
                newName: "Key");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStarted",
                table: "_userTrackingSessions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeEnded",
                table: "_userTrackingSessions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Campus",
                table: "_userTrackingSessions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDateTime",
                table: "_userTrackingInstances",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "CoordinateId",
                table: "_userTrackingInstances",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK__userTrackingInstances_Coordinate_CoordinateId",
                table: "_userTrackingInstances",
                column: "CoordinateId",
                principalTable: "Coordinate",
                principalColumn: "Id");
        }
    }
}
