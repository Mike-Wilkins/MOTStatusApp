using System.ComponentModel.DataAnnotations;

namespace MOTStatusWebApi.Models
{
    public class MOTTestCertificateDetails
    {
       
        public int Id { get; set; }
        public string? MOTTestNumber { get; set; }
        public string? Make { get; set; }
        public string? OdometerReading { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Model { get; set; }
        public string? TestClass { get; set; }
        public string? VehicleID { get; set; }
        public string? Colour { get; set; }
        public string? DateOfRegistration { get; set; }
        public string? MOTDueDate { get; set; }
        public string? DateOfLastMOT { get; set; }
        public string? FuelType { get; set; }
        public string? AuthenticationNumber { get; set; }
        public string? DesignGrossWeight { get; set; }
        public string? AdvisoryNoticeIssued { get; set; }
        public string? TestStationNumber { get; set; }
        public string? SeatBeltChecked { get; set; }
        public string? NumberOfSeatBelts { get; set; }
        public string? PreviousInstallationCheck { get; set; }
        public string? IssuersName { get; set; }
        public string? VTSNumber { get; set; }
        public string? InspectionAuthority { get; set; }
    }
}
