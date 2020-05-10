﻿// <auto-generated />
using Jeopardy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jeopardy.Data.Migrations
{
    [DbContext(typeof(JeopardyContext))]
    [Migration("20200510170951_AddUserTable")]
    partial class AddUserTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Jeopardy.Models.Answer", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AnswerText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AnswerValue")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("HasBeenRead")
                        .HasColumnType("bit");

                    b.HasKey("AnswerId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("Jeopardy.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategorySortOrder")
                        .HasColumnType("int");

                    b.Property<string>("CategoryTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("Round")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.HasIndex("GameId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Jeopardy.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CurrentRound")
                        .HasColumnType("int");

                    b.Property<string>("GameDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameTitle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GameId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Jeopardy.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnswerId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionId");

                    b.HasIndex("AnswerId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Jeopardy.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("UserCorrectAnswers")
                        .HasColumnType("int");

                    b.Property<bool>("UserIsHost")
                        .HasColumnType("bit");

                    b.Property<int>("UserScore")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("GameId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Jeopardy.Models.Answer", b =>
                {
                    b.HasOne("Jeopardy.Models.Category", "Category")
                        .WithMany("Answers")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Jeopardy.Models.Category", b =>
                {
                    b.HasOne("Jeopardy.Models.Game", "Game")
                        .WithMany("Categories")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Jeopardy.Models.Question", b =>
                {
                    b.HasOne("Jeopardy.Models.Answer", "Answer")
                        .WithMany("Questions")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Jeopardy.Models.User", b =>
                {
                    b.HasOne("Jeopardy.Models.Game", "Game")
                        .WithMany("Users")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
