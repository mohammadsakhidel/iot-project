using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class FieldsAddedToTracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "display_name",
                table: "trackers",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "icon_image_id",
                table: "trackers",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "serial_number",
                table: "trackers",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "display_name",
                table: "trackers");

            migrationBuilder.DropColumn(
                name: "icon_image_id",
                table: "trackers");

            migrationBuilder.DropColumn(
                name: "serial_number",
                table: "trackers");
        }
    }
}
