using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CliveBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class SkillDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SkillId = table.Column<int>(type: "integer", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    Cooldown = table.Column<string>(type: "text", nullable: true),
                    Upgrade = table.Column<string>(type: "text", nullable: true),
                    Mastery = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillDetail_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SkillDetailTechniques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SkillDetailId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillDetailTechniques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillDetailTechniques_SkillDetail_SkillDetailId",
                        column: x => x.SkillDetailId,
                        principalTable: "SkillDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillDetail_SkillId",
                table: "SkillDetail",
                column: "SkillId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillDetailTechniques_SkillDetailId",
                table: "SkillDetailTechniques",
                column: "SkillDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillDetailTechniques");

            migrationBuilder.DropTable(
                name: "SkillDetail");
        }
    }
}
