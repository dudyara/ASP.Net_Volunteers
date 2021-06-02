namespace Volunteers.DB.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc/>
    public partial class AddATO : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTypeOrganization_ActivityTypes_ActivityTypesId",
                table: "ActivityTypeOrganization");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTypeOrganization_Organizations_OrganizationsId",
                table: "ActivityTypeOrganization");

            migrationBuilder.RenameColumn(
                name: "OrganizationsId",
                table: "ActivityTypeOrganization",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "ActivityTypesId",
                table: "ActivityTypeOrganization",
                newName: "ActivityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityTypeOrganization_OrganizationsId",
                table: "ActivityTypeOrganization",
                newName: "IX_ActivityTypeOrganization_OrganizationId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "7711cab7-1e19-4de7-92b2-8e4bfa2ff691");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "9e6a83df-344e-4994-a9ca-628e84e8efcc");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTypeOrganization_ActivityTypes_ActivityTypeId",
                table: "ActivityTypeOrganization",
                column: "ActivityTypeId",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTypeOrganization_Organizations_OrganizationId",
                table: "ActivityTypeOrganization",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTypeOrganization_ActivityTypes_ActivityTypeId",
                table: "ActivityTypeOrganization");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTypeOrganization_Organizations_OrganizationId",
                table: "ActivityTypeOrganization");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "ActivityTypeOrganization",
                newName: "OrganizationsId");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeId",
                table: "ActivityTypeOrganization",
                newName: "ActivityTypesId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityTypeOrganization_OrganizationId",
                table: "ActivityTypeOrganization",
                newName: "IX_ActivityTypeOrganization_OrganizationsId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "f8bbf07e-f885-476f-9742-7bdfe49f883c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "0753016b-1834-40fc-bdc2-8ae7a0e79b94");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTypeOrganization_ActivityTypes_ActivityTypesId",
                table: "ActivityTypeOrganization",
                column: "ActivityTypesId",
                principalTable: "ActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTypeOrganization_Organizations_OrganizationsId",
                table: "ActivityTypeOrganization",
                column: "OrganizationsId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
