﻿// <auto-generated />
using System;
using Etiqa_Assessment_REST_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Etiqa_Assessment_REST_API.Migrations
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20240820163130_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Etiqa_Assessment_REST_API.Models.User", b =>
                {
                    b.Property<string>("username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("hobby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("phonenumber")
                        .HasColumnType("int");

                    b.Property<string>("skillsets")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("username");

                    b.ToTable("users");
                });
#pragma warning restore 612, 618
        }
    }
}
