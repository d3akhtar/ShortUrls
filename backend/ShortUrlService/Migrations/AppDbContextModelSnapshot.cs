﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShortUrlService.Data;

#nullable disable

namespace ShortUrlService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShortUrlService.Model.ShortUrl", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DestinationUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAlias")
                        .HasColumnType("bit");

                    b.HasKey("Code");

                    b.ToTable("ShortUrls");

                    b.HasData(
                        new
                        {
                            Code = "test",
                            DestinationUrl = "https://www.youtube.com/watch?v=UyPnhOpngRA&ab_channel=Toast",
                            IsAlias = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
