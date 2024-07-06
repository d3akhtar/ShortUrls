﻿// <auto-generated />
using AuthService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240706005212_MakeHashedPasswordNullable")]
    partial class MakeHashedPasswordNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthService.Model.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HashedPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = "fc989b46-c6f3-424b-9791-3ce43cff0a90",
                            Email = "admin@d3akhtar.com",
                            HashedPassword = "$2a$11$Rbe7xECSirbfYcvLtBffVeVWe0WwdZoO.i4fdnuB3qRBlt9/iO48u",
                            Role = "admin",
                            Username = "d3akhtar-admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
