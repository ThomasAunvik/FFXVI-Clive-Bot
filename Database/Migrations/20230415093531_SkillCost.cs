using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliveBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class SkillCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Skills_MasteredVersionForeignKey",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_MasteredVersionForeignKey",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "MasteredVersionForeignKey",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "MasterizationPoints",
                table: "Skills",
                newName: "CostUpgrade");

            migrationBuilder.AddColumn<int>(
                name: "CostBuy",
                table: "Skills",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostMaster",
                table: "Skills",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostBuy",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CostMaster",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "CostUpgrade",
                table: "Skills",
                newName: "MasterizationPoints");

            migrationBuilder.AddColumn<int>(
                name: "MasteredVersionForeignKey",
                table: "Skills",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_MasteredVersionForeignKey",
                table: "Skills",
                column: "MasteredVersionForeignKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Skills_MasteredVersionForeignKey",
                table: "Skills",
                column: "MasteredVersionForeignKey",
                principalTable: "Skills",
                principalColumn: "Id");
        }
    }
}
