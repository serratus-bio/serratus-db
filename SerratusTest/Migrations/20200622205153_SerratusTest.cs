using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SerratusTest.Migrations
{
    public partial class SerratusTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentLines",
                columns: table => new
                {
                    CommentLineId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sra = table.Column<string>(nullable: true),
                    Genome = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLines", x => x.CommentLineId);
                });

            migrationBuilder.CreateTable(
                name: "AccessionSections",
                columns: table => new
                {
                    AccessionSectionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessionSectionLineId = table.Column<int>(nullable: false),
                    Acc = table.Column<string>(nullable: true),
                    PctId = table.Column<double>(nullable: false),
                    Aln = table.Column<int>(nullable: false),
                    Glb = table.Column<int>(nullable: false),
                    Len = table.Column<int>(nullable: false),
                    CvgPct = table.Column<int>(nullable: false),
                    Depth = table.Column<double>(nullable: false),
                    Cvg = table.Column<string>(nullable: true),
                    Fam = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CommentLineId = table.Column<int>(nullable: false),
                    FamilySectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessionSections", x => x.AccessionSectionId);
                    table.ForeignKey(
                        name: "FK_AccessionSections_CommentLines_CommentLineId",
                        column: x => x.CommentLineId,
                        principalTable: "CommentLines",
                        principalColumn: "CommentLineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilySections",
                columns: table => new
                {
                    FamilySectionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FamilySectionLineId = table.Column<int>(nullable: false),
                    Family = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    PctId = table.Column<int>(nullable: false),
                    Aln = table.Column<int>(nullable: false),
                    Glb = table.Column<int>(nullable: false),
                    PanLen = table.Column<int>(nullable: false),
                    Cvg = table.Column<string>(nullable: true),
                    Top = table.Column<string>(nullable: true),
                    TopAln = table.Column<int>(nullable: false),
                    TopLen = table.Column<int>(nullable: false),
                    TopName = table.Column<string>(nullable: true),
                    CommentLineId = table.Column<int>(nullable: false),
                    AccessionSectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilySections", x => x.FamilySectionId);
                    table.ForeignKey(
                        name: "FK_FamilySections_CommentLines_CommentLineId",
                        column: x => x.CommentLineId,
                        principalTable: "CommentLines",
                        principalColumn: "CommentLineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessionSections_CommentLineId",
                table: "AccessionSections",
                column: "CommentLineId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilySections_CommentLineId",
                table: "FamilySections",
                column: "CommentLineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessionSections");

            migrationBuilder.DropTable(
                name: "FamilySections");

            migrationBuilder.DropTable(
                name: "CommentLines");
        }
    }
}
