using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballResultsApi.Migrations
{
    /// <inheritdoc />
    public partial class favouriteadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeagueUser",
                columns: table => new
                {
                    LeaguesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueUser", x => new { x.LeaguesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_LeagueUser_Leagues_LeaguesId",
                        column: x => x.LeaguesId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeagueUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamUser",
                columns: table => new
                {
                    TeamsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamUser", x => new { x.TeamsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_TeamUser_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueUser_UsersId",
                table: "LeagueUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUser_UsersId",
                table: "TeamUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueUser");

            migrationBuilder.DropTable(
                name: "TeamUser");
        }
    }
}
