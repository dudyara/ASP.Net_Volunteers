namespace Volunteers.DB.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

    /// <inheritdoc/>
    public partial class AddCommentsAndPhones : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestPriority",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                });

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

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationPhoneNumber");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Requests");

            migrationBuilder.AddColumn<long>(
                name: "RequestPriority",
                table: "Requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: string.Empty);
        }
    }
}
