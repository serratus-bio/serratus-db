using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SerratusTest.Migrations
{
    public partial class Summary2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "run",
                columns: table => new
                {
                    run_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(nullable: false),
                    sra_id = table.Column<string>(nullable: false),
                    date = table.Column<string>(nullable: false),
                    version = table.Column<string>(nullable: false),
                    genome = table.Column<string>(nullable: false),
                    read_length = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_run", x => x.run_id);
                });

            migrationBuilder.CreateTable(
                name: "family",
                columns: table => new
                {
                    family_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    family_line = table.Column<int>(nullable: false),
                    sra_id = table.Column<string>(nullable: false),
                    family_name = table.Column<string>(nullable: false),
                    score = table.Column<int>(nullable: false),
                    percent_identity = table.Column<int>(nullable: false),
                    coverage_bins = table.Column<string>(nullable: false),
                    n_reads = table.Column<int>(nullable: false),
                    n_global_reads = table.Column<int>(nullable: false),
                    length = table.Column<int>(nullable: false),
                    depth = table.Column<double>(nullable: false),
                    top_genbank_id = table.Column<string>(nullable: false),
                    top_name = table.Column<string>(nullable: false),
                    top_score = table.Column<int>(nullable: false),
                    top_length = table.Column<int>(nullable: false),
                    run_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_family", x => x.family_id);
                    table.ForeignKey(
                        name: "FK_family_run_run_id",
                        column: x => x.run_id,
                        principalTable: "run",
                        principalColumn: "run_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sequence",
                columns: table => new
                {
                    sequence_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence_line = table.Column<int>(nullable: false),
                    sra_id = table.Column<string>(nullable: false),
                    genbank_id = table.Column<string>(nullable: false),
                    genbank_name = table.Column<string>(nullable: false),
                    family_name = table.Column<string>(nullable: false),
                    score = table.Column<int>(nullable: false),
                    percentage_identity = table.Column<int>(nullable: false),
                    coverage_bins = table.Column<string>(nullable: false),
                    n_reads = table.Column<int>(nullable: false),
                    n_global_reads = table.Column<int>(nullable: false),
                    length = table.Column<int>(nullable: false),
                    depth = table.Column<double>(nullable: false),
                    family_id = table.Column<int>(nullable: true),
                    run_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sequence", x => x.sequence_id);
                    table.ForeignKey(
                        name: "FK_sequence_family_family_id",
                        column: x => x.family_id,
                        principalTable: "family",
                        principalColumn: "family_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sequence_run_run_id",
                        column: x => x.run_id,
                        principalTable: "run",
                        principalColumn: "run_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_family_run_id",
                table: "family",
                column: "run_id");

            migrationBuilder.CreateIndex(
                name: "IX_sequence_family_id",
                table: "sequence",
                column: "family_id");

            migrationBuilder.CreateIndex(
                name: "IX_sequence_run_id",
                table: "sequence",
                column: "run_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sequence");

            migrationBuilder.DropTable(
                name: "family");

            migrationBuilder.DropTable(
                name: "run");
        }
    }
}
