using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "terminals",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    add_time = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    delete_time = table.Column<DateTime>(nullable: false),
                    device_type = table.Column<string>(maxLength: 32, nullable: false),
                    associated_product_id = table.Column<int>(nullable: true),
                    last_connection = table.Column<DateTime>(nullable: true),
                    last_connected_server = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_terminals", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "terminals");
        }
    }
}
