using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace MOTStatusWebApi.Data
{
    public class MOTStatusDetails
    {
        [Name("Id")]
        public int Id { get; set; }

        [Name("RegistrationNumber")]
        [Display(Name ="Registration Number")]
        [Required(ErrorMessage = "Registration number is required")]  
        [RegularExpression(@"^(?=.{1,7})(([a-zA-Z]?){1,3}(\d){1,4}([a-zA-Z]?){1,3})$",
            ErrorMessage = "Special characters and spaces are not allowed")]
        public string? RegistrationNumber { get; set; }

        [Name("Make")]
        [Required(ErrorMessage = "Make is required")]
        public string? Make { get; set; }

        [Name("DateOfRegistration")]
        [Display(Name ="Date of Registration")]
        [Required(ErrorMessage = "Date of Registration is required")]
        public string?  DateOfRegistration { get; set; }

        [Name("CylinderCapacity")]
        [Display(Name="Cylinder Capacity")]
        [Required(ErrorMessage = "Cylinder Capacity is required")]
        public string? CylinderCapacity { get; set; }

        [Name("CO2Emissions")]
        [Display(Name ="CO2 Emissions")]
        [Required(ErrorMessage = "CO2 Emissions is required")]
        public string? CO2Emissions { get; set; }

        [Name("FuelType")]
        [Display(Name = "Fuel Type")]
        [Required(ErrorMessage = "Fuel Type is required")]
        public string? FuelType { get; set; }

        [Name("EuroStatus")]
        [Display(Name = "Euro Status")]
        [Required(ErrorMessage = "Euro Status is required")]
        public string? EuroStatus { get; set; }

        [Name("RealDrivingEmissions")]
        [Display(Name = "Real Driving Emissions")]
        [Required(ErrorMessage = "Real Driving Emissions is required")]
        public string? RealDrivingEmissions { get; set; }

        [Name("ExportMarker")]
        [Display(Name = "Export Marker")]
        [Required(ErrorMessage = "Export Marker is required")]
        public string? ExportMarker { get; set; }

        [Name("VehicleStatus")]
        [Display(Name = "Vehicle Status")]
        public string? VehicleStatus { get; set; }

        [Name("VehicleColour")]
        [Display(Name = "Vehicle Colour")]
        [Required(ErrorMessage = "Vehicle Colour is required")]
        public string? VehicleColour { get; set; }

        [Name("VehicleTypeApproval")]
        [Display(Name = "Vehicle Type Approval")]
        [Required(ErrorMessage = "Vehicle Type Approval is required")]
        public string? VehicleTypeApproval { get; set; }

        [Name("WheelPlan")]
        [Display(Name = "Wheel Plan")]
        [Required(ErrorMessage = "Wheel Plan is required")]
        public string? WheelPlan { get; set; }

        [Name("RevenueWeight")]
        [Display(Name = "Revenue Weight")]
        [Required(ErrorMessage = "Revenue Weight is required")]
        public string? RevenueWeight { get; set; }

        [Name("DateOfLastV5C")]
        [Display(Name = "Date of last V5C")]
        [Required(ErrorMessage = "Date of last VS5 is required")]
        public string? DateOfLastV5C { get; set; }

        [Name("Taxed")]
        public bool Taxed { get; set; }

        [Name("TaxDueDate")]
        [Display(Name = "Tax Due Date")]
        public string TaxDueDate { get; set; }

        [Name("MOTed")]
        public bool MOTed { get; set; }

        [Name("MOTDueDate")]
        [Display(Name = "MOT Due Date")]
        public string MOTDueDate { get; set; }

        [Name("DateOfLastMOT")]
        [Display(Name ="Date of last MOT")]
        [Required(ErrorMessage = "Date of last MOT is required")]
        public string? DateOfLastMOT { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public string? Model { get; set; }

        [Display(Name ="Vehicle ID")]
        [Required(ErrorMessage = "Vehicle ID is required")]
        public string? VehicleID { get; set; }

    }
}
