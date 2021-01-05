using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace TrackDataAccess.Migrations
{
    public partial class InheritanceAddedToMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    creation_time = table.Column<DateTime>(nullable: false),
                    tracker_id = table.Column<string>(maxLength: 16, nullable: false),
                    discriminator = table.Column<string>(maxLength: 32, nullable: false),
                    latitude = table.Column<double>(nullable: true),
                    latitude_mark = table.Column<string>(maxLength: 1, nullable: true),
                    longitude = table.Column<double>(nullable: true),
                    longitude_mark = table.Column<string>(maxLength: 1, nullable: true),
                    message_type = table.Column<string>(maxLength: 16, nullable: true),
                    message_time = table.Column<DateTime>(nullable: true),
                    is_valid = table.Column<bool>(nullable: true),
                    speed = table.Column<double>(nullable: true),
                    direction = table.Column<double>(nullable: true),
                    altitude = table.Column<double>(nullable: true),
                    tracker_state = table.Column<string>(maxLength: 64, nullable: true),
                    signal_strength = table.Column<double>(nullable: true),
                    battery = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_trackers_tracker_id",
                        column: x => x.tracker_id,
                        principalTable: "trackers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_messages_tracker_id",
                table: "messages",
                column: "tracker_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    altitude = table.Column<double>(type: "double", nullable: true),
                    battery = table.Column<double>(type: "double", nullable: true),
                    creation_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    direction = table.Column<double>(type: "double", nullable: false),
                    is_valid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    latitude_mark = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    longitude_mark = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    report_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    report_type = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    signal_strength = table.Column<double>(type: "double", nullable: true),
                    speed = table.Column<double>(type: "double", nullable: false),
                    tracker_id = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    tracker_state = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
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
    }
}
