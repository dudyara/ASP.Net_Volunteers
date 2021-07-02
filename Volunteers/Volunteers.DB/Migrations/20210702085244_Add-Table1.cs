namespace Volunteers.DB.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;
    public partial class AddTable1 : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "RegistrationToken");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RegistrationToken");
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "RegistrationToken",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RegistrationToken",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
