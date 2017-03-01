using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GridWithEFCore.Database;

namespace GridWithEFCore.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("GridWithEFCore.Database.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GridWithEFCore.Database.Session", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abtract");

                    b.Property<int?>("CategoryID");

                    b.Property<int>("MinutesDuration");

                    b.Property<string>("Speaker");

                    b.Property<DateTime>("Start");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("GridWithEFCore.Database.Session", b =>
                {
                    b.HasOne("GridWithEFCore.Database.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID");
                });
        }
    }
}
