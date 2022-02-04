﻿// <auto-generated />
using Covid19.Stats.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Covid19.Stats.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220204060806_DateFiled")]
    partial class DateFiled
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("Covid19.Stats.Data.CovidStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Admin2")
                        .HasColumnType("TEXT");

                    b.Property<float>("Case_Fatality_Ratio")
                        .HasColumnType("REAL");

                    b.Property<string>("Combined_key")
                        .HasColumnType("TEXT");

                    b.Property<int>("Confirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Country_Region")
                        .HasColumnType("TEXT");

                    b.Property<int>("Date")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Death")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FIPS")
                        .HasColumnType("TEXT");

                    b.Property<float>("Incident_Rate")
                        .HasColumnType("REAL");

                    b.Property<int>("Last_Update")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Lat")
                        .HasColumnType("REAL");

                    b.Property<float>("Long")
                        .HasColumnType("REAL");

                    b.Property<string>("Province_State")
                        .HasColumnType("TEXT");

                    b.Property<int>("Recovered")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Stats");
                });
#pragma warning restore 612, 618
        }
    }
}