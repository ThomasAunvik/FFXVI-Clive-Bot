using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliveBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class SkillLanguageNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SkillLanguages_Name",
                table: "SkillLanguages",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SkillLanguages_Name",
                table: "SkillLanguages");
        }
    }
}
