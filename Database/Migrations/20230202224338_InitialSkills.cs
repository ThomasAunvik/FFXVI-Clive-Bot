using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliveBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Summon = table.Column<int>(type: "integer", nullable: false),
                    RatingPhysical = table.Column<int>(type: "integer", nullable: false),
                    RatingMagical = table.Column<int>(type: "integer", nullable: false),
                    MasterizationPoints = table.Column<int>(type: "integer", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: true),
                    PreviewImageUrl = table.Column<string>(type: "text", nullable: true),
                    MasteredVersionForeignKey = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Skills_MasteredVersionForeignKey",
                        column: x => x.MasteredVersionForeignKey,
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SkillLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillLanguages_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillLanguages_SkillId",
                table: "SkillLanguages",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_MasteredVersionForeignKey",
                table: "Skills",
                column: "MasteredVersionForeignKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillLanguages");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
