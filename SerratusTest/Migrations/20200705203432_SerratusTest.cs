using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SerratusTest.Migrations
{
    public partial class SerratusTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    RunId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sra = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Genome = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.RunId);
                });

            migrationBuilder.CreateTable(
                name: "AccessionSections",
                columns: table => new
                {
                    AccessionSectionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessionSectionLineId = table.Column<int>(nullable: false),
                    RunId = table.Column<int>(nullable: false),
                    Sra = table.Column<string>(nullable: true),
                    Fam = table.Column<string>(nullable: true),
                    Acc = table.Column<string>(nullable: true),
                    PctId = table.Column<double>(nullable: false),
                    Aln = table.Column<int>(nullable: false),
                    Glb = table.Column<int>(nullable: false),
                    Len = table.Column<int>(nullable: false),
                    CvgPct = table.Column<int>(nullable: false),
                    Depth = table.Column<double>(nullable: false),
                    Cvg = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessionSections", x => x.AccessionSectionId);
                    table.ForeignKey(
                        name: "FK_AccessionSections_Runs_RunId",
                        column: x => x.RunId,
                        principalTable: "Runs",
                        principalColumn: "RunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilySections",
                columns: table => new
                {
                    FamilySectionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FamilySectionLineId = table.Column<int>(nullable: false),
                    RunId = table.Column<int>(nullable: false),
                    Sra = table.Column<string>(nullable: true),
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
                    TopName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilySections", x => x.FamilySectionId);
                    table.ForeignKey(
                        name: "FK_FamilySections_Runs_RunId",
                        column: x => x.RunId,
                        principalTable: "Runs",
                        principalColumn: "RunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FastaSections",
                columns: table => new
                {
                    FastaSectionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FastaSectionLineId = table.Column<int>(nullable: false),
                    RunId = table.Column<int>(nullable: false),
                    Sra = table.Column<string>(nullable: true),
                    SequenceId = table.Column<string>(nullable: true),
                    Sequence = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastaSections", x => x.FastaSectionId);
                    table.ForeignKey(
                        name: "FK_FastaSections_Runs_RunId",
                        column: x => x.RunId,
                        principalTable: "Runs",
                        principalColumn: "RunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessionSections_RunId",
                table: "AccessionSections",
                column: "RunId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilySections_RunId",
                table: "FamilySections",
                column: "RunId");

            migrationBuilder.CreateIndex(
                name: "IX_FastaSections_RunId",
                table: "FastaSections",
                column: "RunId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessionSections");

            migrationBuilder.DropTable(
                name: "FamilySections");

            migrationBuilder.DropTable(
                name: "FastaSections");

            migrationBuilder.DropTable(
                name: "Runs");
        }
    }
}
