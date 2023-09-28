﻿namespace MOTStatusWebApi.Data
{
    public class MOTStatusDetails
    {
        public int Id { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Make { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string? CylinderCapacity { get; set; }
        public string? CO2Emissions { get; set; }
        public string? FuelType { get; set; }
        public string? EuroStatus { get; set; }
        public string? RealDrivingEmissions { get; set; }
        public string? ExportMarker { get; set; }
        public string? VehicleStatus { get; set; }
        public string? VehicleColour { get; set; }
        public string? VehicleTypeApproval { get; set; }
        public string? WheelPlan { get; set; }
        public string? RevenueWeight { get; set; }
        public DateTime DateOfLastV5C { get; set; }
        public bool Taxed { get; set; }
        public DateTime TaxDueDate { get; set; }
        public bool MOTed { get; set; }
        public DateTime MOTDueDate { get; set; }

    }
}
