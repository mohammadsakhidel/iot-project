using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace TrackDataAccess.Migrations
{
    public partial class AlarmReportAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alarm_reports",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
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
                    tracker_state = table.Column<string>(maxLength: 64, nullable: true),
                    tracker_id = table.Column<string>(maxLength: 16, nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_alarm_reports_tracker_id",
                table: "alarm_reports",
                column: "tracker_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alarm_reports");
        }
    }
}
