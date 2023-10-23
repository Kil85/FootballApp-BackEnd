using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballResultsApi.Migrations
{
    /// <inheritdoc />
    public partial class MissSpellFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HassedPassword",
                table: "Users",
                newName: "HashedPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Users",
                newName: "HassedPassword");
        }
    }
}
