﻿// <auto-generated />
using System;
using BackgroundServiceDatabaseCache.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackgroundServiceDatabaseCache.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("BackgroundServiceDatabaseCache.ExchangeRateValues", b =>
                {
                    b.Property<int>("ExchangeRateValuesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ExchangeRatesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Rate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("ExchangeRateValuesId");

                    b.HasIndex("ExchangeRatesId");

                    b.ToTable("ExchangeRateValues");
                });

            modelBuilder.Entity("BackgroundServiceDatabaseCache.ExchangeRates", b =>
                {
                    b.Property<int>("ExchangeRatesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Base")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ExchangeRatesId");

                    b.ToTable("ExchangeRates");
                });

            modelBuilder.Entity("BackgroundServiceDatabaseCache.ExchangeRateValues", b =>
                {
                    b.HasOne("BackgroundServiceDatabaseCache.ExchangeRates", null)
                        .WithMany("Rates")
                        .HasForeignKey("ExchangeRatesId");
                });

            modelBuilder.Entity("BackgroundServiceDatabaseCache.ExchangeRates", b =>
                {
                    b.Navigation("Rates");
                });
#pragma warning restore 612, 618
        }
    }
}
