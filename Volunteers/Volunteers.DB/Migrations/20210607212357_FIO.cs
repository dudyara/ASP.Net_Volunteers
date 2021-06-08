namespace Volunteers.DB.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc/>
    public partial class FIO : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Requests",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "FinishDate",
                table: "Requests",
                newName: "Complited");

            migrationBuilder.RenameColumn(
                name: "FIO",
                table: "Requests",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ChiefFIO",
                table: "Organizations",
                newName: "Manager");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "994501ef-3485-4ba8-a396-b37293c26645");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "04193422-eaed-4251-8f12-27a63c2230dd");
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Requests",
                newName: "FIO");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Requests",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Complited",
                table: "Requests",
                newName: "FinishDate");

            migrationBuilder.RenameColumn(
                name: "Manager",
                table: "Organizations",
                newName: "ChiefFIO");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "946cd3e6-d9f8-4bb8-9c50-fb492f4c33e6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "622f2905-188a-4fc5-b381-7f92549d2ac1");
        }
    }
}
