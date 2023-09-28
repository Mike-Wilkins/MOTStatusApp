﻿// <auto-generated />
using System;
using MOTStatusWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MOTStatusWebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MOTStatusWebApi.Data.MOTStatusDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CO2Emissions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CylinderCapacity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfLastV5C")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfRegistration")
                        .HasColumnType("datetime2");

                    b.Property<string>("EuroStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExportMarker")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FuelType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MOTDueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("MOTed")
                        .HasColumnType("bit");

                    b.Property<string>("Make")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RealDrivingEmissions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RevenueWeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TaxDueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Taxed")
                        .HasColumnType("bit");

                    b.Property<string>("VehicleColour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleTypeApproval")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WheelPlan")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MOTStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
