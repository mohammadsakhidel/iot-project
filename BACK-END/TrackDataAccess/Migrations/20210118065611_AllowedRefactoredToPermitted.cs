using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackDataAccess.Migrations
{
    public partial class AllowedRefactoredToPermitted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tracker_allowed_users");

            migrationBuilder.CreateTable(
                name: "tracker_permitted_users",
                columns: table => new
                {
                    tracker_id = table.Column<string>(maxLength: 32, nullable: false),
                    user_id = table.Column<string>(maxLength: 64, nullable: false),
                    permissions = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracker_permitted_users", x => new { x.user_id, x.tracker_id });
                    table.ForeignKey(
                        name: "FK_tracker_permitted_users_trackers_tracker_id",
                        column: x => x.tracker_id,
                        principalTable: "trackers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tracker_permitted_users_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tracker_permitted_users_tracker_id",
                table: "tracker_permitted_users",
                column: "tracker_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tracker_permitted_users");

            migrationBuilder.CreateTable(
                name: "tracker_allowed_users",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    tracker_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    permissions = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracker_allowed_users", x => new { x.user_id, x.tracker_id });
                    table.ForeignKey(
                        name: "FK_tracker_allowed_users_trackers_tracker_id",
                        column: x => x.tracker_id,
                        principalTable: "trackers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tracker_allowed_users_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tracker_allowed_users_tracker_id",
                table: "tracker_allowed_users",
                column: "tracker_id");
        }
    }
}
