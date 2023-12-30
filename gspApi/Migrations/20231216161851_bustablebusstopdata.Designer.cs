﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using gspAPI.DbContexts;

#nullable disable

namespace gspAPI.Migrations
{
    [DbContext(typeof(MysqlContext))]
    [Migration("20231216161851_bustablebusstopdata")]
    partial class bustablebusstopdata
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("gspAPI.Entities.BusRoute", b =>
                {
                    b.Property<int>("BusRouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("NameLong")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NameShort")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("BusRouteId");

                    b.ToTable("BusRoutes");
                });

            modelBuilder.Entity("gspAPI.Entities.BusStop", b =>
                {
                    b.Property<int>("BusStopId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BusStopName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Lat")
                        .HasColumnType("double");

                    b.Property<double>("Lon")
                        .HasColumnType("double");

                    b.HasKey("BusStopId");

                    b.ToTable("BusStops");
                });

            modelBuilder.Entity("gspAPI.Entities.BusTable", b =>
                {
                    b.Property<int>("BusTableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BusRouteId")
                        .HasColumnType("int");

                    b.Property<int>("BusStopId")
                        .HasColumnType("int");

                    b.Property<int>("Direction")
                        .HasColumnType("int");

                    b.Property<string>("LastUpdated")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("BusTableId");

                    b.HasIndex("BusRouteId");

                    b.HasIndex("BusStopId");

                    b.ToTable("BusTables");
                });

            modelBuilder.Entity("gspAPI.Entities.BusTrip", b =>
                {
                    b.Property<int>("BusTripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BusRouteId")
                        .HasColumnType("int");

                    b.Property<string>("BusStripNameAlt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("BusTripDirection")
                        .HasColumnType("int");

                    b.Property<string>("BusTripName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("BusTripId");

                    b.HasIndex("BusRouteId");

                    b.ToTable("BusTrips");
                });

            modelBuilder.Entity("gspAPI.Entities.BusTripBusStop", b =>
                {
                    b.Property<int>("BusTripBusStopId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BusStopId")
                        .HasColumnType("int");

                    b.Property<int>("BusTripId")
                        .HasColumnType("int");

                    b.Property<int>("Direction")
                        .HasColumnType("int");

                    b.HasKey("BusTripBusStopId");

                    b.HasIndex("BusStopId");

                    b.HasIndex("BusTripId");

                    b.ToTable("BusTripBusStop");
                });

            modelBuilder.Entity("gspAPI.Entities.DayType", b =>
                {
                    b.Property<int>("DayTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("DayTypeId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("DayType");

                    b.HasData(
                        new
                        {
                            DayTypeId = 1,
                            Name = "Workday"
                        },
                        new
                        {
                            DayTypeId = 2,
                            Name = "Saturday"
                        },
                        new
                        {
                            DayTypeId = 3,
                            Name = "Sunday"
                        });
                });

            modelBuilder.Entity("gspAPI.Entities.PingCache", b =>
                {
                    b.Property<int>("PingCacheId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BusTableId")
                        .HasColumnType("int");

                    b.Property<int>("Distance")
                        .HasColumnType("int");

                    b.Property<float>("Lat")
                        .HasColumnType("float");

                    b.Property<float>("Lon")
                        .HasColumnType("float");

                    b.Property<int>("TimeId")
                        .HasColumnType("int");

                    b.HasKey("PingCacheId");

                    b.HasIndex("BusTableId");

                    b.HasIndex("TimeId");

                    b.ToTable("PingCache");
                });

            modelBuilder.Entity("gspAPI.Entities.Time", b =>
                {
                    b.Property<int>("TimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DayTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Hour")
                        .HasColumnType("int");

                    b.Property<int>("Minute")
                        .HasColumnType("int");

                    b.HasKey("TimeId");

                    b.HasIndex("DayTypeId");

                    b.ToTable("Time");
                });

            modelBuilder.Entity("gspAPI.Entities.TimeBusTable", b =>
                {
                    b.Property<int>("BusTableId")
                        .HasColumnType("int");

                    b.Property<int>("TimeId")
                        .HasColumnType("int");

                    b.HasKey("BusTableId", "TimeId");

                    b.HasIndex("TimeId");

                    b.ToTable("TimeBusTable");
                });

            modelBuilder.Entity("gspAPI.Entities.BusTable", b =>
                {
                    b.HasOne("gspAPI.Entities.BusRoute", "BusRoute")
                        .WithMany()
                        .HasForeignKey("BusRouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("gspAPI.Entities.BusStop", "BusStop")
                        .WithMany()
                        .HasForeignKey("BusStopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusRoute");

                    b.Navigation("BusStop");
                });

            modelBuilder.Entity("gspAPI.Entities.BusTrip", b =>
                {
                    b.HasOne("gspAPI.Entities.BusRoute", "BusRoute")
                        .WithMany()
                        .HasForeignKey("BusRouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusRoute");
                });

            modelBuilder.Entity("gspAPI.Entities.BusTripBusStop", b =>
                {
                    b.HasOne("gspAPI.Entities.BusStop", null)
                        .WithMany()
                        .HasForeignKey("BusStopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("gspAPI.Entities.BusTrip", null)
                        .WithMany()
                        .HasForeignKey("BusTripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("gspAPI.Entities.PingCache", b =>
                {
                    b.HasOne("gspAPI.Entities.BusTable", "BusTable")
                        .WithMany()
                        .HasForeignKey("BusTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("gspAPI.Entities.Time", "Time")
                        .WithMany()
                        .HasForeignKey("TimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusTable");

                    b.Navigation("Time");
                });

            modelBuilder.Entity("gspAPI.Entities.Time", b =>
                {
                    b.HasOne("gspAPI.Entities.DayType", "DayType")
                        .WithMany("Minutes")
                        .HasForeignKey("DayTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DayType");
                });

            modelBuilder.Entity("gspAPI.Entities.TimeBusTable", b =>
                {
                    b.HasOne("gspAPI.Entities.BusTable", null)
                        .WithMany()
                        .HasForeignKey("BusTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("gspAPI.Entities.Time", null)
                        .WithMany()
                        .HasForeignKey("TimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("gspAPI.Entities.DayType", b =>
                {
                    b.Navigation("Minutes");
                });
#pragma warning restore 612, 618
        }
    }
}
