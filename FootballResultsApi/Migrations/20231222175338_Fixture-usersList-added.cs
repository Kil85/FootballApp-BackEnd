using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballResultsApi.Migrations
{
    /// <inheritdoc />
    public partial class FixtureusersListadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FixtureId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FixtureId",
                table: "Users",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Fixtures_FixtureId",
                table: "Users",
                column: "FixtureId",
                principalTable: "Fixtures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Fixtures_FixtureId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FixtureId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FixtureId",
                table: "Users");
        }
    }
}
