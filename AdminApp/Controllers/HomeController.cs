
using AdminApp.Interfaces;
using AdminApp.ViewModels;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace AdminApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMOTTestCertificateDetailsRepository _testDetailsRepository;
        private readonly IMOTStatusDetailsRepository _statusDetailsRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMOTStatusViewData _viewData;

        public HomeController(ILogger<HomeController> logger, IMOTTestCertificateDetailsRepository testDetailsRepository, IMOTStatusDetailsRepository statusDetailsRepository, IHostingEnvironment hostingEnvironment, IMOTStatusViewData viewData)
        {
            _logger = logger;
            _statusDetailsRepository = statusDetailsRepository;
            _hostingEnvironment = hostingEnvironment;
            _viewData = viewData;
            _testDetailsRepository = testDetailsRepository;
           
        }
        public IActionResult Index()
        {               
            return View(_viewData);
        }

        [HttpPost]
        public IActionResult Index(string registration)
        {      
            if(registration == null)
            {
                _viewData.RegistrationValidationError = true;
                return View(_viewData);
            }

            registration = registration.ToUpper().Replace(" ", "");

            var regexValidation = VehicleRegEx(registration);          
            
            if(!regexValidation)
            {
                _viewData.RegistrationFormatError = true;
                return View(_viewData);
            }

            var details = _statusDetailsRepository.GetStatusDetails().
                Where(d => d.RegistrationNumber == registration.ToUpper()).FirstOrDefault();

            if (details == null)
            {
                _viewData.RegistrationFormatError = true;
                _viewData.RegistrationValidationError = true;
                _viewData.RegistrationNotFoundError = true;
                return View(_viewData);
            }

            return RedirectToAction("Menu", new {registration = registration});
        }

        public IActionResult Menu(string registration)
        {
            var details = _statusDetailsRepository.GetStatusDetails().
                Where(d => d.RegistrationNumber == registration.ToUpper()).FirstOrDefault();

            string formatReg = FormatReg(details.RegistrationNumber);
            ViewBag.FormatReg = formatReg;

            return View(details);
        }

        public static string FormatReg(string registration)
        {
            var result = registration.Insert(4, " ");
            return result;
        }

        public static bool VehicleRegEx(string registration)
        {
            var regExIsValid = Regex.IsMatch(registration, @"^(?=.{1,7})(([a-zA-Z]?){1,3}(\d){1,4}([a-zA-Z]?){1,3})$");

            return regExIsValid;
        }
        public IActionResult Delete(int Id)
        {
            var details = _statusDetailsRepository.GetStatusDetails().
              Where(d => d.Id == Id).FirstOrDefault();

            _viewData.mOTStatusDetails = details;

            string formatReg = FormatReg(_viewData.mOTStatusDetails.RegistrationNumber);
            _viewData.FormatReg = formatReg;

            return View(_viewData);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteVehicle(int Id)
        {
            var details = _statusDetailsRepository.GetStatusDetails().
                Where(d => d.Id == Id).FirstOrDefault();

            _statusDetailsRepository.Delete(details);

            string formatReg = FormatReg(details.RegistrationNumber);
            _viewData.FormatReg = formatReg;
            _viewData.VehicleDeleted = true;

            return View(_viewData);
        }

        public IActionResult Update(int Id)
        {
            var details = _statusDetailsRepository.GetStatusDetails().
               Where(d => d.Id == Id).FirstOrDefault();      

            details = FormatDatePicker(details);

            return View(details);
        }

        [HttpPost]
        public IActionResult Update(MOTStatusDetails details)
        {
            ModelState.Remove(nameof(details.VehicleStatus));

            if (!ModelState.IsValid)
            {
                return View(details);
            }

            details = FormatObjectDetails(details);

            _statusDetailsRepository.Update(details);

            return RedirectToAction("Menu", new { registration = details.RegistrationNumber });
        }

        public IActionResult Create()
        {
            ViewBag.RegIsUnique = true;
            return View();
        }

        [HttpPost]
        public IActionResult Create(MOTStatusDetails details)
        {
            ModelState.Remove(nameof(details.TaxDueDate));
            ModelState.Remove(nameof(details.MOTDueDate));
            ModelState.Remove(nameof(details.DateOfLastMOT));

            //details.RegistrationNumber = details.RegistrationNumber.ToUpper().Replace(" ", "");

            if (!ModelState.IsValid)
            {
                return View(details);
          
            }

            var regIsUnique = RegistrationIsUnique(details.RegistrationNumber);

            if(regIsUnique == false)
            {
                ViewBag.RegIsUnique = false;
                return View();
            }

            DateTime registrastionDate = DateTime.Parse(details.DateOfRegistration);

            //Vehicles OLDER than 3 years
            if(DateTime.Now >= registrastionDate.AddYears(3))
            {
                var MOTList = _testDetailsRepository.GetTestCertificateDetails().Where(x => x.VehicleID == details.VehicleID).ToList();

                if (MOTList.Count == 0)
                {
                    details.MOTDueDate = registrastionDate.AddYears(3).ToString("dd/MM/yyyy");
                    details.DateOfLastMOT = details.DateOfRegistration;
                }
                else
                {
                    var latestMOTCert = MOTList.Max(m => m.MOTTestNumber);
                    var mOTDueDate = _testDetailsRepository.GetTestCertificateDetails().Where(d => d.MOTTestNumber == latestMOTCert).FirstOrDefault();
                    details.MOTDueDate = mOTDueDate.MOTDueDate;
                    details.DateOfLastMOT = mOTDueDate.DateOfLastMOT;
                }

            }

            //Vehicles LESS than 3 years old do not require MOT
            if (DateTime.Now <= registrastionDate.AddYears(3))
            {
                details.MOTDueDate = registrastionDate.AddYears(3).ToString();
                details.DateOfLastMOT = details.DateOfRegistration;
            }

            details = FormatObjectDetails(details);


            _statusDetailsRepository.Add(details);

            return RedirectToAction("Menu", new { registration = details.RegistrationNumber });
        }

        public static MOTStatusDetails FormatObjectDetails(MOTStatusDetails details)
        {
            details.DateOfRegistration = FormatDate(details.DateOfRegistration); 
            details.DateOfLastV5C = FormatDate(details.DateOfLastV5C);
            details.DateOfLastMOT = FormatDate(details.DateOfLastMOT);
            details.TaxDueDate = UpdateVehicleDueDate(details.DateOfLastV5C);
            details.Taxed = IsVehicleTaxedAndMOTed(details.TaxDueDate);
            details.MOTed = IsVehicleTaxedAndMOTed(details.MOTDueDate);
            details.VehicleStatus = IsTaxedLabel(details.Taxed);

            return (details);
        }

        public static string IsTaxedLabel(bool taxed)
        {
            if (taxed)
            {
                return "Taxed";
            }
            return "Not Taxed";
        }

        public static string FormatDate(string detailsDate)
        {
            DateTime date = DateTime.Parse(detailsDate);
            var result = date.ToString("dd/MM/yyyy");

            return (result);
        }

        public static string UpdateVehicleDueDate(string date)
        {
            DateTime currentDate = DateTime.Parse(date);
            DateTime dueDate = currentDate.AddYears(1);
            var result = dueDate.ToString("dd/MM/yyyy");

            return (result);
        }

        public static bool IsVehicleTaxedAndMOTed(string date)
        {
            DateTime dueDate = DateTime.Parse(date);

            if (dueDate < DateTime.Now)
            {
                return false;
            }
            return (true);
        }

        public static MOTStatusDetails FormatDatePicker(MOTStatusDetails details)
        {
            string[] detailDates = { details.DateOfRegistration, details.DateOfLastV5C, details.DateOfLastMOT };

            for (int i = 0; i < detailDates.Length; i++)
            {
                detailDates[i] = DateTime.Parse(detailDates[i]).ToString("yyyy-MM-dd");
            }

            details.DateOfRegistration = detailDates[0];
            details.DateOfLastV5C = detailDates[1];
            details.DateOfLastMOT = detailDates[2];

            return (details);
        }
        public bool RegistrationIsUnique(string registration)
        {
            var details = _statusDetailsRepository.GetStatusDetails().
                FirstOrDefault(d => d.RegistrationNumber == registration);

            if(details != null)
            {
                return false;
            }

            return (true);
        }

        public IActionResult UploadCSV()
        {
            return View(_viewData);
        }

        [HttpPost]
        public IActionResult UploadCSV(IFormFile file)
        {
            string filePath = null;
            string uniqueFileName = null;

            if(file != null)
            {
                if (file.ContentType != "text/csv")
                {
                    _viewData.IncorrectFileType = true;
                    return View(_viewData);
                }

                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "csvfiles");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var csvFile = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(csvFile);
                }  

                using (var reader = new StreamReader(filePath))
                {
                    using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        try
                        {
                            var records = csvReader.GetRecords<MOTStatusDetails>().ToList();
                           var recordErrors = CSVIsFormattedCorrectly(records);
                            csvReader.Dispose();

                            if (recordErrors.FileErrorFound == true)
                            {
                                System.IO.File.Delete(filePath);
                                return View(_viewData);
                            }

                            foreach (var record in records)
                            {
                                var formatedDetailsObject = FormatObjectDetails(record);
                                _statusDetailsRepository.Add(formatedDetailsObject);
                            }
                            _viewData.RecordUploadCount = records.Count();
                        }  
                        catch (Exception ex)
                        {
                            csvReader.Dispose();
                            System.IO.File.Delete(filePath);
                            _viewData.CSVFileFormatError = true;
                          
                            return View(_viewData);
                        }
                    }
                }

                System.IO.File.Delete(filePath);
            }
            else
            {
                _viewData.CSVFileNullError = true;
                return View(_viewData);
            }

            
            _viewData.FileUploadSuccess = true;
            return View(_viewData);
        }

        public MOTStatusViewData CSVIsFormattedCorrectly(List<MOTStatusDetails> records)
        {
            //bool fileErrorFound = false;
            List<string> registrationNumbers = new List<string>();

            foreach (var record in records)
            {
                if (record.DateOfLastMOT == "NULL")
                {
                    record.DateOfLastMOT = record.DateOfRegistration;
                }

                if (record.MOTDueDate == "NULL")
                {
                    DateTime registrastionDate = DateTime.Parse(record.DateOfRegistration);
                    record.MOTDueDate = registrastionDate.AddYears(3).ToString("dd/MM/yyyy");
                }

                registrationNumbers.Add(record.RegistrationNumber);

                var dateOfRegistrationresult = Regex.IsMatch(record.DateOfRegistration, @"^(?<day>\d\d?)/(?<month>\d\d?)/(?<year>\d\d\d\d)$");
                var dateOfLastMOTresult = Regex.IsMatch(record.DateOfLastMOT, @"^(?<day>\d\d?)/(?<month>\d\d?)/(?<year>\d\d\d\d)$");
                var dateOfLastV5Cresult = Regex.IsMatch(record.DateOfLastV5C, @"^(?<day>\d\d?)/(?<month>\d\d?)/(?<year>\d\d\d\d)$");
                var regExIsValid = Regex.IsMatch(record.RegistrationNumber, @"^(?=.{1,7})(([a-zA-Z]?){1,3}(\d){1,4}([a-zA-Z]?){1,3})$");

                if (record.Id != 0 ||
                    !regExIsValid ||
                    !dateOfRegistrationresult ||
                    !dateOfLastMOTresult ||
                    !dateOfLastV5Cresult ||
                    String.IsNullOrEmpty(record.Make) ||
                    String.IsNullOrEmpty(record.CylinderCapacity) ||
                    String.IsNullOrEmpty(record.CO2Emissions) ||
                    String.IsNullOrEmpty(record.FuelType) ||
                    String.IsNullOrEmpty(record.WheelPlan) ||
                    String.IsNullOrEmpty(record.EuroStatus) ||
                    String.IsNullOrEmpty(record.RealDrivingEmissions) ||
                    String.IsNullOrEmpty(record.ExportMarker) ||
                    String.IsNullOrEmpty(record.VehicleColour) ||
                    String.IsNullOrEmpty(record.VehicleTypeApproval) ||
                    String.IsNullOrEmpty(record.RevenueWeight) ||
                    String.IsNullOrEmpty(record.Model) ||
                    String.IsNullOrEmpty(record.VehicleID)
                   )
                {
                    _viewData.CSVFileFormatError = true;
                    _viewData.FileErrorFound = true;
                }

                var details = _statusDetailsRepository.GetStatusDetails().
                                FirstOrDefault(d => d.RegistrationNumber == record.RegistrationNumber);

                if (details != null || record.RegistrationNumber.Length > 7)
                {
                    _viewData.RegistrationError = true;
                   _viewData.FileErrorFound = true;
                }
            }

            bool isUnique = registrationNumbers.Distinct().Count() == registrationNumbers.Count();
            if (!isUnique)
            {
                _viewData.RegistrationError = true;
                _viewData.FileErrorFound = true;
            }

             return ((MOTStatusViewData)_viewData);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}