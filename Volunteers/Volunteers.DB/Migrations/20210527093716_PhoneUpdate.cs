namespace Volunteers.DB.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc/>
    public partial class PhoneUpdate : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationPhoneNumber");

            migrationBuilder.AddColumn<long>(
                name: "OrganizationId",
                table: "PhoneNumber",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_OrganizationId",
                table: "PhoneNumber",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumber_Organizations_OrganizationId",
                table: "PhoneNumber",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumber_Organizations_OrganizationId",
                table: "PhoneNumber");

            migrationBuilder.DropIndex(
                name: "IX_PhoneNumber_OrganizationId",
                table: "PhoneNumber");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "PhoneNumber");

            migrationBuilder.CreateTable(
                name: "OrganizationPhoneNumber",
                columns: table => new
                {
                    OrganizationsId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumbersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationPhoneNumber", x => new { x.OrganizationsId, x.PhoneNumbersId });
                    table.ForeignKey(
                        name: "FK_OrganizationPhoneNumber_Organizations_OrganizationsId",
                        column: x => x.OrganizationsId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationPhoneNumber_PhoneNumber_PhoneNumbersId",
                        column: x => x.PhoneNumbersId,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPhoneNumber_PhoneNumbersId",
                table: "OrganizationPhoneNumber",
                column: "PhoneNumbersId");
        }
    }
}
