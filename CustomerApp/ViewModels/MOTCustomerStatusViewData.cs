using CustomerApp.Interfaces;
using MOTStatusWebApi.Data;

namespace CustomerApp.ViewModels
{
    public class MOTCustomerStatusViewData : IMOTCustomerStatusViewData
    {
        public MOTStatusDetails mOTStatusDetails { get; set; }
        public bool RegistrationValidationError { get; set; }
        public bool RegistrationFormatError { get; set; }
        public bool RegistrationNotFoundError { get; set; }
        public bool ConfirmNotSelectedError { get; set; }
    }
}
