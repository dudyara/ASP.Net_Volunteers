namespace Volunteers.DB.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc/>
    public partial class Seed : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1L, "f8bbf07e-f885-476f-9742-7bdfe49f883c", "Admin", null },
                    { 2L, "0753016b-1834-40fc-bdc2-8ae7a0e79b94", "Organization", null }
                });
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
