using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace TrackDataAccess.Migrations
{
    public partial class AlarmAndLocMerged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alarm_reports");

            migrationBuilder.DropTable(
                name: "location_reports");

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    creation_time = table.Column<DateTime>(nullable: false),
                    report_type = table.Column<string>(maxLength: 16, nullable: false),
                    tracker_id = table.Column<string>(maxLength: 16, nullable: false),
                    latitude = table.Column<double>(nullable: false),
                    latitude_mark = table.Column<string>(maxLength: 1, nullable: false),
                    longitude = table.Column<double>(nullable: false),
                    longitude_mark = table.Column<string>(maxLength: 1, nullable: false),
                    is_valid = table.Column<bool>(nullable: false),
                    speed = table.Column<double>(nullable: false),
                    direction = table.Column<double>(nullable: false),
                    altitude = table.Column<double>(nullable: true),
                    signal_strength = table.Column<double>(nullable: true),
                    battery = table.Column<double>(nullable: true),
                    tracker_state = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.id);
                    table.ForeignKey(
                        name: "FK_reports_trackers_tracker_id",
                        column: x => x.tracker_id,
                        principalTable: "trackers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reports_tracker_id",
                table: "reports",
                column: "tracker_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.CreateTable(
                name: "alarm_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    altitude = table.Column<double>(type: "double", nullable: true),
                    battery = table.Column<double>(type: "double", nullable: true),
                    creation_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    direction = table.Column<double>(type: "double", nullable: false),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    latitude_mark = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    longitude_mark = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    signal_strength = table.Column<double>(type: "double", nullable: true),
                    speed = table.Column<double>(type: "double", nullable: false),
                    tracker_id = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    tracker_state = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alarm_reports", x => x.id);
                    table.ForeignKey(
                        name: "FK_alarm_reports_trackers_tracker_id",
                        column: x => x.tracker_id,
                        principalTable: "trackers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "location_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    altitude = table.Column<double>(type: "double", nullable: true),
                    battery = table.Column<double>(type: "double", nullable: true),
                    creation_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    direction = table.Column<double>(type: "double", nullable: false),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    latitude_mark = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    longitude_mark = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    signal_strength = table.Column<double>(type: "double", nullable: true),
                    speed = table.Column<double>(type: "double", nullable: false),
                    tracker_id = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    tracker_state = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
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
                name: "IX_alarm_reports_tracker_id",
                table: "alarm_reports",
                column: "tracker_id");

            migrationBuilder.CreateIndex(
                name: "IX_location_reports_tracker_id",
                table: "location_reports",
                column: "tracker_id");
        }
    }
}
