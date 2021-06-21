namespace Volunteers.DB.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// New_relationship5
    /// </summary>
    public partial class New_relationship5 : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RegistrationTokenId",
                table: "Organizations",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "a435ddf4-e22a-40f3-a687-4199d184a253");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "6294e81c-23c7-4ad4-a38a-f14cfd800ebd");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_RegistrationTokenId",
                table: "Organizations",
                column: "RegistrationTokenId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_RegistrationToken_RegistrationTokenId",
                table: "Organizations",
                column: "RegistrationTokenId",
                principalTable: "RegistrationToken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_RegistrationToken_RegistrationTokenId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_RegistrationTokenId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "RegistrationTokenId",
                table: "Organizations");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "d93b36ee-e4e5-4dce-b80b-7b47e169ea54");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "58d8aa0f-77e0-4e9e-b6de-13d044f6db75");
        }
    }
}
