using System.ComponentModel.DataAnnotations;

namespace MOTStatusWebApi.Models
{
    public class MOTTestCertificateDetails
    {
       
        public int Id { get; set; }

        [Display(Name = "MOT Test Number")]
        public string? MOTTestNumber { get; set; }


        public string? Make { get; set; }

        [Display(Name = "Odometer Reading")]
        [Required(ErrorMessage = "Odometer Reading is Required")]
        public string? OdometerReading { get; set; }


        [Display(Name = "Registration Number")]
        public string? RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Model is Required")]
        public string? Model { get; set; }


        [Display(Name = "Test Class")]
        [Required(ErrorMessage = "Test Class is Required")]
        public string? TestClass { get; set; }

        [Display(Name = "Vehicle ID")]
        [Required(ErrorMessage = "Vehicle ID is Required")]
        public string? VehicleID { get; set; }


        public string? Colour { get; set; }

        [Display(Name = "Date of Registration")]
        public string? DateOfRegistration { get; set; }

        [Display(Name = "Test Passed?")]
        public string? TestResult { get; set; }

        [Display(Name = "Reason(s) for vehicle failure")]
        [Required(ErrorMessage = "MOT failure details are required")]
        public string? ReasonForFailure { get; set; }
        public string? AdvisoryNotice { get; set; }

        [Display(Name = "MOT Due Data")]
        public string? MOTDueDate { get; set; }

        [Display(Name = "Issue Date/Time")]
        public string? DateOfLastMOT { get; set; }

        [Display(Name = "Fuel Type")]
        public string? FuelType { get; set; }

        [Display(Name = "Authentication Number")]
        [Required(ErrorMessage = "Authentication Number is Required")]
        public string? AuthenticationNumber { get; set; }

        [Display(Name = "Design Gross Weight")]
        [Required(ErrorMessage = "Design Gross Weight is Required")]
        public string? DesignGrossWeight { get; set; }

        [Display(Name = "Advisory Notice Issued?")]
        [Required(ErrorMessage = "Advisory Notice Issued is Required")]
        public string? AdvisoryNoticeIssued { get; set; }

        [Display(Name = "Test Station Number")]
        [Required(ErrorMessage = "Test Station Number is Required")]
        public string? TestStationNumber { get; set; }

        [Display(Name = "Seat Belt Checked?")]

        public string? SeatBeltChecked { get; set; }

        [Display(Name = "Number of Seat Belts")]
        public string? NumberOfSeatBelts { get; set; }

        [Display(Name = "Previous Installation Check")]
        public string? PreviousInstallationCheck { get; set; }

        [Display(Name = "Issuer's Name")]
        [Required(ErrorMessage = "Issuers Name is Required")]
        public string? IssuersName { get; set; }

        [Display(Name = "VTS Number")]
        [Required(ErrorMessage = "VTS Number is Required")]
        public string? VTSNumber { get; set; }

        [Display(Name = "Inspection Authority")]
        [Required(ErrorMessage = "Inspection Authority is Required")]
        public string? InspectionAuthority { get; set; }
    }
}
