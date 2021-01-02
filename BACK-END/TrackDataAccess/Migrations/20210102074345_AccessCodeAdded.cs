using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class AccessCodeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_codes",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 64, nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<string>(maxLength: 64, nullable: false),
                    tracker_id = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_codes", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_codes");
        }
    }
}
