namespace Volunteers.DB.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

    /// <inheritdoc/>
    public partial class ATO_id : Migration
    {

        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityTypeOrganization",
                table: "ActivityTypeOrganization");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ActivityTypeOrganization",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityTypeOrganization",
                table: "ActivityTypeOrganization",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypeOrganization_ActivityTypeId",
                table: "ActivityTypeOrganization",
                column: "ActivityTypeId");
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityTypeOrganization",
                table: "ActivityTypeOrganization");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTypeOrganization_ActivityTypeId",
                table: "ActivityTypeOrganization");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ActivityTypeOrganization");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityTypeOrganization",
                table: "ActivityTypeOrganization",
                columns: new[] { "ActivityTypeId", "OrganizationId" });
        }
    }
}
