﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotesAPI.Models;

namespace NotesAPI.Migrations
{
    [DbContext(typeof(NotesAPIContext))]
    [Migration("20180805164514_initialCreate")]
    partial class initialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NotesAPI.Models.CheckedList", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ListItem");

                    b.Property<int?>("NotesID");

                    b.HasKey("ID");

                    b.HasIndex("NotesID");

                    b.ToTable("CheckedList");
                });

            modelBuilder.Entity("NotesAPI.Models.Labels", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Label");

                    b.Property<int?>("NotesID");

                    b.HasKey("ID");

                    b.HasIndex("NotesID");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("NotesAPI.Models.Notes", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Pinned");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("NotesAPI.Models.CheckedList", b =>
                {
                    b.HasOne("NotesAPI.Models.Notes")
                        .WithMany("CheckedList")
                        .HasForeignKey("NotesID");
                });

            modelBuilder.Entity("NotesAPI.Models.Labels", b =>
                {
                    b.HasOne("NotesAPI.Models.Notes")
                        .WithMany("Label")
                        .HasForeignKey("NotesID");
                });
#pragma warning restore 612, 618
        }
    }
}
