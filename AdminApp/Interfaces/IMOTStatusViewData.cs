using MOTStatusWebApi.Data;

namespace AdminApp.Interfaces
{
    public interface IMOTStatusViewData
    {
        public MOTStatusDetails mOTStatusDetails { get; set; }
        public bool RegistrationValidationError { get; set; }
        public bool RegistrationFormatError { get; set; }
        public bool RegistrationNotFoundError { get; set; }
        public bool RegIsUnique { get; set; }

        public bool IncorrectFileType { get; set; }
        public bool CSVFileNullError { get; set; }
        public bool CSVFileFormatError  { get; set; }
        public bool FileUploadSuccess { get; set; }
        public int RecordUploadCount { get; set; }
        public bool RegistrationError { get; set; }
        public bool FileErrorFound { get; set; }
        public string FormatReg { get; set; }
        public bool VehicleDeleted { get; set; }

    }
}
