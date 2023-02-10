using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CliveBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class BotModAndCharacterInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotModeratorPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ManageModerators = table.Column<bool>(type: "boolean", nullable: false),
                    AllPermissions = table.Column<bool>(type: "boolean", nullable: false),
                    ManageSkills = table.Column<bool>(type: "boolean", nullable: false),
                    ManageSkillInfo = table.Column<bool>(type: "boolean", nullable: false),
                    ManageSkillTranslations = table.Column<bool>(type: "boolean", nullable: false),
                    ManageCharacters = table.Column<bool>(type: "boolean", nullable: false),
                    ManageCharacterInfo = table.Column<bool>(type: "boolean", nullable: false),
                    ManageCharacterNotes = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotModeratorPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotModerators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ConnectionSource = table.Column<string>(type: "text", nullable: false),
                    ConnectionId = table.Column<string>(type: "text", nullable: false),
                    PermissionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotModerators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotModerators_BotModeratorPermissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "BotModeratorPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotModerators_PermissionsId",
                table: "BotModerators",
                column: "PermissionsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotModerators");

            migrationBuilder.DropTable(
                name: "BotModeratorPermissions");
        }
    }
}
