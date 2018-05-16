﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notes.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Notes.Persistence.Migrations
{
    [DbContext(typeof(NoteContext))]
    partial class NoteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Notes.Model.Note", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Text");

                    b.Property<DateTime>("Updated");

                    b.HasKey("Id");

                    b.ToTable("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
