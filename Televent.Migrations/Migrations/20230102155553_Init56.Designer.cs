﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Televent.Data;

#nullable disable

namespace Televent.Migrations.Migrations
{
    [DbContext(typeof(TeleventContext))]
    [Migration("20230102155553_Init56")]
    partial class Init56
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Televent.Core.Events.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("event_description");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("event_name");

                    b.Property<DateTimeOffset>("ExecutionTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("execution_time");

                    b.Property<bool>("IsExecuted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_executed");

                    b.Property<string>("Message")
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.HasKey("Id")
                        .HasName("pk_events");

                    b.ToTable("events", (string)null);
                });

            modelBuilder.Entity("Televent.Core.Games.Models.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean")
                        .HasColumnName("is_finished");

                    b.Property<int>("PlayersCount")
                        .HasColumnType("integer")
                        .HasColumnName("players_count");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.HasKey("Id")
                        .HasName("pk_games");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("Televent.Core.Users.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("varchar")
                        .HasColumnName("additional_info");

                    b.Property<int?>("Age")
                        .HasColumnType("integer")
                        .HasColumnName("age");

                    b.Property<string>("Building")
                        .HasColumnType("varchar")
                        .HasColumnName("building");

                    b.Property<long?>("ChatId")
                        .HasColumnType("bigint")
                        .HasColumnName("chat_id");

                    b.Property<bool>("IsRegistered")
                        .HasColumnType("boolean")
                        .HasColumnName("is_registered");

                    b.Property<string>("NameAndSurname")
                        .HasColumnType("text")
                        .HasColumnName("name_and_surname");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<int?>("Room")
                        .HasColumnType("integer")
                        .HasColumnName("room");

                    b.Property<int?>("Squad")
                        .HasColumnType("integer")
                        .HasColumnName("squad");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<long?>("WardId")
                        .HasColumnType("bigint")
                        .HasColumnName("ward_id");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
