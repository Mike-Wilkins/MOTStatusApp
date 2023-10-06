using System.ComponentModel.DataAnnotations;

namespace MOTStatusWebApi.Data
{
    public class MOTStatusDetails
    {
        public int Id { get; set; }

        [Display(Name ="Registration Number")]
        [Required(ErrorMessage = "Registration number is required")]
        public string? RegistrationNumber { get; set; }
        [Required(ErrorMessage = "Make is required")]
        public string? Make { get; set; }

        [Display(Name ="Date of Registration")]
        [Required(ErrorMessage = "Date of Registration is required")]
        public string?  DateOfRegistration { get; set; }

        [Display(Name="Cylinder Capacity")]
        [Required(ErrorMessage = "Cylinder Capacity is required")]
        public string? CylinderCapacity { get; set; }

        [Display(Name ="CO2 Emissions")]
        [Required(ErrorMessage = "CO2 Emissions is required")]
        public string? CO2Emissions { get; set; }

        [Display(Name = "Fuel Type")]
        [Required(ErrorMessage = "Fuel Type is required")]
        public string? FuelType { get; set; }

        [Display(Name = "Euro Status")]
        [Required(ErrorMessage = "Euro Status is required")]
        public string? EuroStatus { get; set; }

        [Display(Name = "Real Driving Emissions")]
        [Required(ErrorMessage = "Real Driving Emissions is required")]
        public string? RealDrivingEmissions { get; set; }

        [Display(Name = "Export Marker")]
        [Required(ErrorMessage = "Export Marker is required")]
        public string? ExportMarker { get; set; }

        [Display(Name = "Vehicle Status")]
        public string? VehicleStatus { get; set; }

        [Display(Name = "Vehicle Colour")]
        [Required(ErrorMessage = "Vehicle Colour is required")]
        public string? VehicleColour { get; set; }

        [Display(Name = "Vehicle Type Approval")]
        [Required(ErrorMessage = "Vehicle Type Approval is required")]
        public string? VehicleTypeApproval { get; set; }

        [Display(Name = "Wheel Plan")]
        [Required(ErrorMessage = "Wheel Plan is required")]
        public string? WheelPlan { get; set; }

        [Display(Name = "Revenue Weight")]
        [Required(ErrorMessage = "Revenue Weight is required")]
        public string? RevenueWeight { get; set; }

        [Display(Name = "Date of last V5C")]
        [Required(ErrorMessage = "Date of last VS5 is required")]
        public string? DateOfLastV5C { get; set; }
        public bool Taxed { get; set; }

        [Display(Name = "Tax Due Date")]
        public string TaxDueDate { get; set; }
        public bool MOTed { get; set; }

        [Display(Name = "MOT Due Date")]
        public string MOTDueDate { get; set; }

        [Display(Name ="Date of last MOT")]
        [Required(ErrorMessage = "Date of last MOT is required")]
        public string? DateOfLastMOT { get; set; }

    }
}
