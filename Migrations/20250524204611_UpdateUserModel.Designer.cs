﻿// <auto-generated />
using System;
using Dashboard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dashboard.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250524204611_UpdateUserModel")]
    partial class UpdateUserModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dashboard.Models.DashboardParam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("ActualOutput")
                        .HasColumnType("float");

                    b.Property<string>("ControlNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ConveyorSpeed")
                        .HasColumnType("float");

                    b.Property<double>("CycleTime")
                        .HasColumnType("float");

                    b.Property<string>("Family")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plant")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Project")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TactTime")
                        .HasColumnType("float");

                    b.Property<double>("TargetQuantity")
                        .HasColumnType("float");

                    b.Property<double>("WorkingTime")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("DashboardParams");
                });

            modelBuilder.Entity("Dashboard.Models.Station", b =>
                {
                    b.Property<int>("StationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StationId"));

                    b.Property<double>("AverageAwtValue")
                        .HasColumnType("float");

                    b.Property<double>("AverageGumValue")
                        .HasColumnType("float");

                    b.Property<double>("AwtValue")
                        .HasColumnType("float");

                    b.Property<double>("DirectOperator")
                        .HasColumnType("float");

                    b.Property<double>("GumValue")
                        .HasColumnType("float");

                    b.Property<double>("IndirectOperator")
                        .HasColumnType("float");

                    b.Property<double>("MaxAwtValue")
                        .HasColumnType("float");

                    b.Property<double>("MaxGumValue")
                        .HasColumnType("float");

                    b.Property<double>("MinAwtValue")
                        .HasColumnType("float");

                    b.Property<double>("MinGumValue")
                        .HasColumnType("float");

                    b.Property<string>("PartNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("StationId");

                    b.HasIndex("UserId");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("Dashboard.Models.StationAWT", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("StationAWTs");
                });

            modelBuilder.Entity("Dashboard.Models.StationGUM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("StationGUMs");
                });

            modelBuilder.Entity("Dashboard.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Dashboard.Models.Station", b =>
                {
                    b.HasOne("Dashboard.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dashboard.Models.StationAWT", b =>
                {
                    b.HasOne("Dashboard.Models.Station", "Station")
                        .WithMany("AWTEntries")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("Dashboard.Models.StationGUM", b =>
                {
                    b.HasOne("Dashboard.Models.Station", "Station")
                        .WithMany("GUMEntries")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("Dashboard.Models.Station", b =>
                {
                    b.Navigation("AWTEntries");

                    b.Navigation("GUMEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
