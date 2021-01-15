using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class PermissionsAddedToAllowedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "permissions",
                table: "tracker_allowed_users",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permissions",
                table: "tracker_allowed_users");
        }
    }
}
