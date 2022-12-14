// <auto-generated />
using CityInfo.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CityInfo.API.Migrations
{
    [DbContext(typeof(CityInfoContext))]
    [Migration("20220716155455_CityInfoPopulateDataMigration")]
    partial class CityInfoPopulateDataMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("CityInfo.API.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Odessa City",
                            Name = "Odessa"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Khrkiv City",
                            Name = "Kharkiv"
                        },
                        new
                        {
                            Id = 3,
                            Description = "I love watermelons",
                            Name = "Kherson"
                        });
                });

            modelBuilder.Entity("CityInfo.API.Entities.PointOfInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PointOfInterests");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            Description = "Visit at night",
                            Name = "Opera theater"
                        },
                        new
                        {
                            Id = 2,
                            CityId = 1,
                            Description = "Good to ride a bake",
                            Name = "Shevchenko Park"
                        },
                        new
                        {
                            Id = 3,
                            CityId = 1,
                            Description = "Criminal district",
                            Name = "Poskot"
                        },
                        new
                        {
                            Id = 4,
                            CityId = 2,
                            Description = "Something to buy",
                            Name = "Saltovka"
                        },
                        new
                        {
                            Id = 5,
                            CityId = 2,
                            Description = "Nice place to stutdy",
                            Name = "University"
                        },
                        new
                        {
                            Id = 6,
                            CityId = 3,
                            Description = "Nice place to raise a meeting",
                            Name = "Downtown"
                        });
                });

            modelBuilder.Entity("CityInfo.API.Entities.PointOfInterest", b =>
                {
                    b.HasOne("CityInfo.API.Entities.City", "City")
                        .WithMany("PointsOfInterest")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("CityInfo.API.Entities.City", b =>
                {
                    b.Navigation("PointsOfInterest");
                });
#pragma warning restore 612, 618
        }
    }
}
