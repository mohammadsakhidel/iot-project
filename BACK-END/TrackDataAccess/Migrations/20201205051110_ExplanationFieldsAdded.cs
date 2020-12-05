using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class ExplanationFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "explanation",
                table: "trackers",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "product_model",
                table: "trackers",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "explanation",
                table: "AspNetUsers",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "explanation",
                table: "trackers");

            migrationBuilder.DropColumn(
                name: "product_model",
                table: "trackers");

            migrationBuilder.DropColumn(
                name: "explanation",
                table: "AspNetUsers");
        }
    }
}
