namespace Volunteers.DB.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc/>
    public partial class ActivityTypesUpdate : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTypes_Organizations_OrganizationId",
                table: "ActivityTypes");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTypes_OrganizationId",
                table: "ActivityTypes");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "ActivityTypes");

            migrationBuilder.CreateTable(
                name: "ActivityTypeOrganization",
                columns: table => new
                {
                    ActivityTypesId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTypeOrganization", x => new { x.ActivityTypesId, x.OrganizationsId });
                    table.ForeignKey(
                        name: "FK_ActivityTypeOrganization_ActivityTypes_ActivityTypesId",
                        column: x => x.ActivityTypesId,
                        principalTable: "ActivityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityTypeOrganization_Organizations_OrganizationsId",
                        column: x => x.OrganizationsId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypeOrganization_OrganizationsId",
                table: "ActivityTypeOrganization",
                column: "OrganizationsId");
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityTypeOrganization");

            migrationBuilder.AddColumn<long>(
                name: "OrganizationId",
                table: "ActivityTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypes_OrganizationId",
                table: "ActivityTypes",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTypes_Organizations_OrganizationId",
                table: "ActivityTypes",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
