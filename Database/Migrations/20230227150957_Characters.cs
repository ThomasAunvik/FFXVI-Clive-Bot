using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CliveBot.Database.Migrations
{
    /// <inheritdoc />
    public partial class Characters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    NoteName = table.Column<string>(type: "text", nullable: false),
                    NoteDescription = table.Column<string>(type: "text", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: false),
                    PreviewImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterNotes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DefaultVariant = table.Column<bool>(type: "boolean", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    PreviewImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterVariants_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterVariantFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    VariantId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterVariantFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterVariantFields_CharacterVariants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "CharacterVariants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterNotes_CharacterId",
                table: "CharacterNotes",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterVariantFields_VariantId",
                table: "CharacterVariantFields",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterVariants_CharacterId",
                table: "CharacterVariants",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterNotes");

            migrationBuilder.DropTable(
                name: "CharacterVariantFields");

            migrationBuilder.DropTable(
                name: "CharacterVariants");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
