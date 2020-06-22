﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SerratusTest.ORM;

namespace SerratusTest.Migrations
{
    [DbContext(typeof(SerratusSummaryContext))]
    partial class SerratusSummaryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SerratusTest.Domain.Model.AccessionSection", b =>
                {
                    b.Property<int>("AccessionSectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Acc")
                        .HasColumnType("text");

                    b.Property<int>("AccessionSectionLineId")
                        .HasColumnType("integer");

                    b.Property<int>("Aln")
                        .HasColumnType("integer");

                    b.Property<int>("CommentLineId")
                        .HasColumnType("integer");

                    b.Property<string>("Cvg")
                        .HasColumnType("text");

                    b.Property<string>("CvgPct")
                        .HasColumnType("text");

                    b.Property<int>("Depth")
                        .HasColumnType("integer");

                    b.Property<string>("Fam")
                        .HasColumnType("text");

                    b.Property<int>("FamilySectionId")
                        .HasColumnType("integer");

                    b.Property<int>("Glb")
                        .HasColumnType("integer");

                    b.Property<int>("Len")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PctId")
                        .HasColumnType("integer");

                    b.HasKey("AccessionSectionId");

                    b.HasIndex("CommentLineId");

                    b.ToTable("AccessionSections");
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.CommentLine", b =>
                {
                    b.Property<int>("CommentLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Date")
                        .HasColumnType("text");

                    b.Property<string>("Genome")
                        .HasColumnType("text");

                    b.Property<string>("Sra")
                        .HasColumnType("text");

                    b.HasKey("CommentLineId");

                    b.ToTable("CommentLines");
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.FamilySection", b =>
                {
                    b.Property<int>("FamilySectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccessionSectionId")
                        .HasColumnType("integer");

                    b.Property<int>("Aln")
                        .HasColumnType("integer");

                    b.Property<int>("CommentLineId")
                        .HasColumnType("integer");

                    b.Property<string>("Cvg")
                        .HasColumnType("text");

                    b.Property<string>("Family")
                        .HasColumnType("text");

                    b.Property<int>("FamilySectionLineId")
                        .HasColumnType("integer");

                    b.Property<int>("Glb")
                        .HasColumnType("integer");

                    b.Property<int>("PanLen")
                        .HasColumnType("integer");

                    b.Property<int>("PctId")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<string>("Top")
                        .HasColumnType("text");

                    b.Property<int>("TopAln")
                        .HasColumnType("integer");

                    b.Property<int>("TopLen")
                        .HasColumnType("integer");

                    b.Property<string>("TopName")
                        .HasColumnType("text");

                    b.HasKey("FamilySectionId");

                    b.HasIndex("CommentLineId");

                    b.ToTable("FamilySections");
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.AccessionSection", b =>
                {
                    b.HasOne("SerratusTest.Domain.Model.CommentLine", null)
                        .WithMany("AccessionSections")
                        .HasForeignKey("CommentLineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.FamilySection", b =>
                {
                    b.HasOne("SerratusTest.Domain.Model.CommentLine", null)
                        .WithMany("FamilySections")
                        .HasForeignKey("CommentLineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}