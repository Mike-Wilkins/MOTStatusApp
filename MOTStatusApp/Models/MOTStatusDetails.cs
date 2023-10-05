using System.ComponentModel.DataAnnotations;

namespace MOTStatusWebApi.Data
{
    public class MOTStatusDetails
    {
        public int Id { get; set; }

        [Display(Name ="Registration Number")]
        public string? RegistrationNumber { get; set; }

        public string? Make { get; set; }

        [Display(Name ="Date of Registration")]
        public string?  DateOfRegistration { get; set; }

        [Display(Name="Cylinder Capacity")]
        public string? CylinderCapacity { get; set; }

        [Display(Name ="CO2 Emissions")]
        public string? CO2Emissions { get; set; }

        [Display(Name = "Fuel Type")]
        public string? FuelType { get; set; }

        [Display(Name = "Euro Status")]
        public string? EuroStatus { get; set; }

        [Display(Name = "Real Driving Emissions")]
        public string? RealDrivingEmissions { get; set; }

        [Display(Name = "Export Marker")]
        public string? ExportMarker { get; set; }

        [Display(Name = "Vehicle Status")]
        public string? VehicleStatus { get; set; }

        [Display(Name = "Vehicle Colour")]
        public string? VehicleColour { get; set; }

        [Display(Name = "Vehicle Type Approval")]
        public string? VehicleTypeApproval { get; set; }

        [Display(Name = "Wheel Plan")]
        public string? WheelPlan { get; set; }

        [Display(Name = "Revenue Weight")]
        public string? RevenueWeight { get; set; }

        [Display(Name = "Date of last V5C")]
        public string? DateOfLastV5C { get; set; }
        public bool Taxed { get; set; }

        [Display(Name = "Tax Due Date")]
        public string TaxDueDate { get; set; }
        public bool MOTed { get; set; }

        [Display(Name = "MOT Due Date")]
        public string MOTDueDate { get; set; }

        [Display(Name ="Date of last MOT")]
        public string? DateOfLastMOT { get; set; }

    }
}
