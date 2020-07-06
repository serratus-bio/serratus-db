﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SerratusTest.ORM;

namespace SerratusTest.Migrations
{
    [DbContext(typeof(SerratusSummaryContext))]
    [Migration("20200705203432_SerratusTest")]
    partial class SerratusTest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Cvg")
                        .HasColumnType("text");

                    b.Property<int>("CvgPct")
                        .HasColumnType("integer");

                    b.Property<double>("Depth")
                        .HasColumnType("double precision");

                    b.Property<string>("Fam")
                        .HasColumnType("text");

                    b.Property<int>("Glb")
                        .HasColumnType("integer");

                    b.Property<int>("Len")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("PctId")
                        .HasColumnType("double precision");

                    b.Property<int>("RunId")
                        .HasColumnType("integer");

                    b.Property<string>("Sra")
                        .HasColumnType("text");

                    b.HasKey("AccessionSectionId");

                    b.HasIndex("RunId");

                    b.ToTable("AccessionSections");
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.FamilySection", b =>
                {
                    b.Property<int>("FamilySectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Aln")
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

                    b.Property<int>("RunId")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<string>("Sra")
                        .HasColumnType("text");

                    b.Property<string>("Top")
                        .HasColumnType("text");

                    b.Property<int>("TopAln")
                        .HasColumnType("integer");

                    b.Property<int>("TopLen")
                        .HasColumnType("integer");

                    b.Property<string>("TopName")
                        .HasColumnType("text");

                    b.HasKey("FamilySectionId");

                    b.HasIndex("RunId");

                    b.ToTable("FamilySections");
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.FastaSection", b =>
                {
                    b.Property<int>("FastaSectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("FastaSectionLineId")
                        .HasColumnType("integer");

                    b.Property<int>("RunId")
                        .HasColumnType("integer");

                    b.Property<string>("Sequence")
                        .HasColumnType("text");

                    b.Property<string>("SequenceId")
                        .HasColumnType("text");

                    b.Property<string>("Sra")
                        .HasColumnType("text");

                    b.HasKey("FastaSectionId");

                    b.HasIndex("RunId");

                    b.ToTable("FastaSections");
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.Run", b =>
                {
                    b.Property<int>("RunId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Date")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("Genome")
                        .HasColumnType("text");

                    b.Property<string>("Sra")
                        .HasColumnType("text");

                    b.HasKey("RunId");

                    b.ToTable("Runs");
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.AccessionSection", b =>
                {
                    b.HasOne("SerratusTest.Domain.Model.Run", null)
                        .WithMany("AccessionSections")
                        .HasForeignKey("RunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.FamilySection", b =>
                {
                    b.HasOne("SerratusTest.Domain.Model.Run", null)
                        .WithMany("FamilySections")
                        .HasForeignKey("RunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SerratusTest.Domain.Model.FastaSection", b =>
                {
                    b.HasOne("SerratusTest.Domain.Model.Run", null)
                        .WithMany("FastaSections")
                        .HasForeignKey("RunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}