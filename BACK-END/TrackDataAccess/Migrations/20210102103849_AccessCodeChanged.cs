using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class AccessCodeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tracker_id",
                table: "access_codes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tracker_id",
                table: "access_codes",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }
    }
}
