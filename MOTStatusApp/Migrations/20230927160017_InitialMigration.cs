using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MOTStatusWebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOTStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CylinderCapacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CO2Emissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EuroStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealDrivingEmissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExportMarker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleColour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTypeApproval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WheelPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevenueWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfLastV5C = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Taxed = table.Column<bool>(type: "bit", nullable: false),
                    TaxDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MOTed = table.Column<bool>(type: "bit", nullable: false),
                    MOTDueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTStatus", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOTStatus");
        }
    }
}
