using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class ImageDbSetAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 64, nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 32, nullable: false),
                    bytes = table.Column<byte[]>(type: "MEDIUMBLOB", nullable: false),
                    width = table.Column<int>(nullable: false),
                    height = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "images");
        }
    }
}
