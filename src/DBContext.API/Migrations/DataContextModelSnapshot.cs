﻿// <auto-generated />
using DBContext.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DBContext.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.API.PropertyListing", b =>
                {
                    b.Property<int>("ListingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryType")
                        .HasColumnType("int");

                    b.Property<string>("DisplayPrice")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("PostCode")
                        .HasColumnType("int");

                    b.Property<string>("ShortPrice")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int>("StatusType")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Suburb")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ListingId");

                    b.ToTable("Listings");
                });
#pragma warning restore 612, 618
        }
    }
}
