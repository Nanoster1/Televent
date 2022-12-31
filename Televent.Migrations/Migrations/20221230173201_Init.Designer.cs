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
    [Migration("20221230173201_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Televent.Core.Users.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("text")
                        .HasColumnName("additional_info");

                    b.Property<int?>("Age")
                        .HasColumnType("integer")
                        .HasColumnName("age");

                    b.Property<int?>("Building")
                        .HasColumnType("integer")
                        .HasColumnName("building");

                    b.Property<bool>("IsRegistered")
                        .HasColumnType("boolean")
                        .HasColumnName("is_registered");

                    b.Property<string>("NameAndSurname")
                        .HasColumnType("text")
                        .HasColumnName("name_and_surname");

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

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}