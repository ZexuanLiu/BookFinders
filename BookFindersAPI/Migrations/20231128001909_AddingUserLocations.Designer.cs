﻿// <auto-generated />
using System;
using BookFindersAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookFindersAPI.Migrations
{
    [DbContext(typeof(ProductionDatabase))]
    [Migration("20231128001909_AddingUserLocations")]
    partial class AddingUserLocations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BookFindersLibrary.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BookId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("PostDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ThumbsUp")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("_comment");
                });

            modelBuilder.Entity("BookFindersLibrary.Models.Coordinate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("X")
                        .HasColumnType("real");

                    b.Property<float>("Y")
                        .HasColumnType("real");

                    b.Property<float>("Z")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("_coordinates");
                });

            modelBuilder.Entity("BookFindersLibrary.Models.PushNotification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("_pushNotifications");
                });

            modelBuilder.Entity("BookFindersLibrary.Models.UserLocations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DestinationId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("XCoordinate")
                        .HasColumnType("double precision");

                    b.Property<double>("YCoordinate")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("_locations");
                });

            modelBuilder.Entity("BookFindersLibrary.Models.UserTrackingInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CoordinateId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PostDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UserTrackingSessionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CoordinateId");

                    b.HasIndex("UserTrackingSessionId");

                    b.ToTable("_userTrackingInstances");
                });

            modelBuilder.Entity("BookFindersLibrary.Models.UserTrackingSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Campus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeEnded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TimeStarted")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("_userTrackingSessions");
                });

            modelBuilder.Entity("BookFindersLibrary.Models.UserTrackingInstance", b =>
                {
                    b.HasOne("BookFindersLibrary.Models.Coordinate", "Coordinate")
                        .WithMany()
                        .HasForeignKey("CoordinateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookFindersLibrary.Models.UserTrackingSession", null)
                        .WithMany("Locations")
                        .HasForeignKey("UserTrackingSessionId");

                    b.Navigation("Coordinate");
                });

            modelBuilder.Entity("BookFindersLibrary.Models.UserTrackingSession", b =>
                {
                    b.Navigation("Locations");
                });
#pragma warning restore 612, 618
        }
    }
}