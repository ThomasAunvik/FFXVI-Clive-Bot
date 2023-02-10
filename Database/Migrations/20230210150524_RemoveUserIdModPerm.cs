using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliveBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdModPerm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BotModeratorPermissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BotModeratorPermissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
