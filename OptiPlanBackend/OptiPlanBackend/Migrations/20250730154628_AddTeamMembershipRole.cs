using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiPlanBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamMembershipRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Invitations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Invitations");
        }
    }
}
