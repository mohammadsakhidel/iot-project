using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace TrackDataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trackers",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 16, nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    delete_time = table.Column<DateTime>(nullable: false),
                    device_type = table.Column<string>(maxLength: 32, nullable: false),
                    associated_product_id = table.Column<int>(nullable: true),
                    last_connection = table.Column<DateTime>(nullable: true),
                    last_connected_server = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trackers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "location_reports",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tracker_id = table.Column<string>(maxLength: 16, nullable: false),
                    report_time = table.Column<DateTime>(nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    latitude_mark = table.Column<string>(maxLength: 1, nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    longitude_mark = table.Column<string>(maxLength: 1, nullable: false),
                    speed = table.Column<double>(nullable: false),
                    direction = table.Column<double>(nullable: false),
                    altitude = table.Column<double>(nullable: true),
                    signal_strength = table.Column<double>(nullable: true),
                    battery = table.Column<double>(nullable: true),
                    tracker_state = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location_reports", x => x.id);
                    table.ForeignKey(
                        name: "FK_location_reports_trackers_tracker_id",
                        column: x => x.tracker_id,
                        principalTable: "trackers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_location_reports_tracker_id",
                table: "location_reports",
                column: "tracker_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "location_reports");

            migrationBuilder.DropTable(
                name: "trackers");
        }
    }
}
