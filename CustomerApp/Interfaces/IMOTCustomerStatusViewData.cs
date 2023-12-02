using MOTStatusWebApi.Data;

namespace CustomerApp.Interfaces
{
    public interface IMOTCustomerStatusViewData
    {
        public MOTStatusDetails mOTStatusDetails { get; set; }
        public bool RegistrationValidationError {get; set;}
        public bool RegistrationFormatError {get; set;}
        public bool RegistrationNotFoundError {get; set;}
        public bool ConfirmNotSelectedError {get; set;}
    }
}
