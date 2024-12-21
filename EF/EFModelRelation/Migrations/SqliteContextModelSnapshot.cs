﻿// <auto-generated />
using System;
using EFModelRelation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFModelRelation.Migrations
{
    [DbContext(typeof(SqliteContext))]
    partial class SqliteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("EFModelRelation.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("EFModelRelation.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EFModelRelation.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BlogId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("EFModelRelation.Models.Employee", b =>
                {
                    b.HasOne("EFModelRelation.Models.Employee", "Manager")
                        .WithMany("Reports")
                        .HasForeignKey("ManagerId");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("EFModelRelation.Models.Post", b =>
                {
                    b.HasOne("EFModelRelation.Models.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("EFModelRelation.Models.Blog", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("EFModelRelation.Models.Employee", b =>
                {
                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}
