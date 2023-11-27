using AdminApp.Models;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Collections.Generic;
using System.Linq;

namespace AdminApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMOTStatusDetailsRepository _statusDetailsRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IMOTStatusDetailsRepository statusDetailsRepository, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _statusDetailsRepository = statusDetailsRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {

            ViewBag.RegistrationValidationError = false;
            ViewBag.RegistrationFormatError = false;
            ViewBag.RegistrationNotFoundError = false;
           
            return View();
        }

        [HttpPost]
        public IActionResult Index(string registration)
        {
            
            if(registration == null)
            {
               ViewBag.RegistrationValidationError = true;
               ViewBag.RegistrationFormatError = false;
                return View();
            }

            registration = registration.ToUpper().Replace(" ", "");

            var regexValidation = VehicleRegEx(registration);          
            
            if(!regexValidation)
            {
                ViewBag.RegistrationValidationError = false;
                ViewBag.RegistrationFormatError = true;
                return View();
            }

            var details = _statusDetailsRepository.GetStatusDetails().
                Where(d => d.RegistrationNumber == registration.ToUpper()).FirstOrDefault();

            if (details == null)
            {
                ViewBag.RegistrationNotFoundError = true;
                return View();
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

            string formatReg = FormatReg(details.RegistrationNumber);
            ViewBag.FormatReg = formatReg;

            ViewBag.VehicleDeleted = false;

            details = FormatDatePicker(details);

            return View(details);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteVehicle(int Id)
        {

            var details = _statusDetailsRepository.GetStatusDetails().
                Where(d => d.Id == Id).FirstOrDefault();

            _statusDetailsRepository.Delete(details);

            string formatReg = FormatReg(details.RegistrationNumber);
            ViewBag.FormatReg = formatReg;

            ViewBag.VehicleDeleted = true;

            return View(details);
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

            ModelState.Remove(nameof(details.TaxDueDate));
            ModelState.Remove(nameof(details.MOTDueDate));

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
            ViewBag.RegistrationFormatError = false;
            ViewBag.RegIsUnique = true;
            return View();
        }

        [HttpPost]
        public IActionResult Create(MOTStatusDetails details)
        {
            ModelState.Remove(nameof(details.TaxDueDate));
            ModelState.Remove(nameof(details.MOTDueDate));

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
            details.MOTDueDate = UpdateVehicleDueDate(details.DateOfLastMOT);

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
            ViewBag.IncorrectFileType = false;
            ViewBag.CSVFileNullError = false;
            ViewBag.CSVFileFormatError = false;
            ViewBag.FileUploadSuccess = false;
            return View();
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
                    ViewBag.IncorrectFileType = true;
                    return View();
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
                       var records = csvReader.GetRecords<MOTStatusDetails>().ToList();
                       var recordErrors = CSVIsFormattedCorrectly(records);

                        if(recordErrors == true)
                        {
                            ViewBag.CSVFileFormatError = true;
                            return View();
                        }

                        foreach (var record in records)
                        {
                            var formatedDetailsObject = FormatObjectDetails(record);
                            _statusDetailsRepository.Add(formatedDetailsObject);
                        }
                    }
                }

                System.IO.File.Delete(filePath);
            }
            else
            {
                ViewBag.CSVFileNullError = true;
                return View();
            }

            ViewBag.FileUploadSuccess = true;
            return View();
        }

        public bool CSVIsFormattedCorrectly(List<MOTStatusDetails> records)
        {
            bool fileErrorFound = false;

            foreach(var record in records)
            {
                if (String.IsNullOrEmpty(record.Make) ||
                    String.IsNullOrEmpty(record.CylinderCapacity) ||
                    String.IsNullOrEmpty(record.CO2Emissions) ||
                    String.IsNullOrEmpty(record.FuelType) ||
                    String.IsNullOrEmpty(record.WheelPlan) ||
                    String.IsNullOrEmpty(record.EuroStatus) ||
                    String.IsNullOrEmpty(record.RealDrivingEmissions) ||
                    String.IsNullOrEmpty(record.ExportMarker) ||
                    String.IsNullOrEmpty(record.VehicleColour) ||
                    String.IsNullOrEmpty(record.VehicleTypeApproval) ||          
                    String.IsNullOrEmpty(record.RevenueWeight)
                    )
                {
                    fileErrorFound = true;
                }            
            }

            foreach(var record in records)
            {
             var dateOfRegistrationresult = Regex.IsMatch(record.DateOfRegistration, @"^(?<day>\d\d?)/(?<month>\d\d?)/(?<year>\d\d\d\d)$");
             var dateOfLastMOTresult = Regex.IsMatch(record.DateOfLastMOT, @"^(?<day>\d\d?)/(?<month>\d\d?)/(?<year>\d\d\d\d)$");
             var dateOfLastV5Cresult = Regex.IsMatch(record.DateOfLastV5C, @"^(?<day>\d\d?)/(?<month>\d\d?)/(?<year>\d\d\d\d)$");
             var regExIsValid = Regex.IsMatch(record.RegistrationNumber, @"^(?=.{1,7})(([a-zA-Z]?){1,3}(\d){1,4}([a-zA-Z]?){1,3})$");

                if (record.Id != 0 ||
                    !regExIsValid ||              
                    !dateOfRegistrationresult || 
                    !dateOfLastMOTresult || 
                    !dateOfLastV5Cresult
                    )
                {
                    fileErrorFound = true;
                }
            }

            foreach(var record in records)
            {
                var details = _statusDetailsRepository.GetStatusDetails().
               FirstOrDefault(d => d.RegistrationNumber == record.RegistrationNumber);

                if ( details != null)
                {
                    fileErrorFound = true;
                }
            }

             return (fileErrorFound);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}