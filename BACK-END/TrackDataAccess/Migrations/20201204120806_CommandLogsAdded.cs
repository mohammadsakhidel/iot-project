using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace TrackDataAccess.Migrations
{
    public partial class CommandLogsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "command_logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    creation_time = table.Column<DateTime>(nullable: false),
                    type = table.Column<string>(maxLength: 32, nullable: false),
                    tracker_id = table.Column<string>(maxLength: 32, nullable: false),
                    payload = table.Column<string>(maxLength: 256, nullable: true),
                    user_id = table.Column<string>(maxLength: 64, nullable: false),
                    response = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_command_logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "command_logs");
        }
    }
}
