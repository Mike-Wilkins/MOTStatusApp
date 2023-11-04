using AdminApp.Models;
using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdminApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMOTStatusDetailsRepository _statusDetailsRepository;

        public HomeController(ILogger<HomeController> logger, IMOTStatusDetailsRepository statusDetailsRepository)
        {
            _logger = logger;
            _statusDetailsRepository = statusDetailsRepository;
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
            var regexPattern = @"/^([A-Z]{3}\s?(\d{3}|\d{2}|d{1})\s?[A-Z])|([A-Z]\s?(\d{3}|\d{2}|\d{1})\s?[A-Z]{3})|(([A-HK-PRSVWY][A-HJ-PR-Y])\s?([0][2-9]|[1-9][0-9])\s?[A-HJ-PR-Z]{3})$/";
            var regExIsValid = Regex.IsMatch(registration, regexPattern);

            return regExIsValid;
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
            return View();
        }

        [HttpPost]
        public IActionResult Create(MOTStatusDetails details)
        {
            ModelState.Remove(nameof(details.TaxDueDate));
            ModelState.Remove(nameof(details.MOTDueDate));

            if (!ModelState.IsValid)
            {
                return View(details);
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

            if (details.Taxed)
            {
                details.VehicleStatus = "Taxed";
            }
            else { details.VehicleStatus = "Not Taxed"; }

            return (details);
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




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}