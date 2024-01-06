using CustomerApp.Interfaces;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Models;

namespace CustomerApp.ViewModels
{
    public class MOTCustomerStatusViewData : IMOTCustomerStatusViewData
    {
        public IEnumerable<MOTTestCertificateDetails> mOTTestCertificateDetails { get; set; }
        public MOTTestCertificateDetails mOTTestCertificateDetail { get; set; }
        public MOTStatusDetails mOTStatusDetails { get; set; }
        public bool RegistrationValidationError { get; set; }
        public bool RegistrationFormatError { get; set; }
        public bool RegistrationNotFoundError { get; set; }
        public bool ConfirmNotSelectedError { get; set; }
    }
}
