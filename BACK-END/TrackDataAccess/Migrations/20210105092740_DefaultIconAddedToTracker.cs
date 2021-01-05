using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class DefaultIconAddedToTracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "default_icon",
                table: "trackers",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "default_icon",
                table: "trackers");
        }
    }
}
