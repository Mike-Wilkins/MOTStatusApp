using CustomerApp.Models;
using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CustomerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        HttpClient client = new HttpClient();
        string url = "https://localhost:7132/api/MOTStatusDetails/11/registration?registration=";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.RegistrationValidationError = false;
            ViewBag.RegistrationFormatError = false;
            ViewBag.RegistrationNotFoundError = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string registration)
        {
            if (registration == null)
            {
                ViewBag.RegistrationValidationError = true;
                ViewBag.RegistrationFormatError = false;
                return View();
            }

            var regexValidation = VehicleRegEx(registration);

            if (!regexValidation)
            {
                ViewBag.RegistrationValidationError = false;
                ViewBag.RegistrationFormatError = true;
                return View();
            }

            try
            {
                var carDetail = JsonConvert.DeserializeObject<MOTStatusDetails>(await client.GetStringAsync(url + registration));             
                return RedirectToAction("DetailConfirmation", carDetail );
            }
            catch (Exception ex)
            {
                ViewBag.RegistrationNotFoundError = true;
                return View();
            }        
        }

        public IActionResult DetailConfirmation(MOTStatusDetails carDetails)
        {
            string formatReg = FormatReg(carDetails.RegistrationNumber);
            carDetails.RegistrationNumber = formatReg;
            ViewBag.ConfirmNotSelectedError = false;
            return View(carDetails);
        }

        [HttpPost]
        public IActionResult DetailConfirmation(string confirm, MOTStatusDetails detail)
        {
            if(confirm == null)
            {
                string formatReg = FormatReg(detail.RegistrationNumber);
                detail.RegistrationNumber = formatReg;
                ViewBag.ConfirmNotSelectedError = true;
                return View(detail);
            }

            if(confirm == "Yes")
            {
                return RedirectToAction("VehicleDetail", detail);
            }else if(confirm == "No"){
                return RedirectToAction("Index");
            }

            return View(confirm);
        }

        public IActionResult VehicleDetail(MOTStatusDetails detail)
        {
            string formatReg = FormatReg(detail.RegistrationNumber);
            detail.RegistrationNumber = formatReg;

            detail.TaxDueDate = FormatDate(detail.TaxDueDate);
            detail.MOTDueDate = FormatDate(detail.MOTDueDate);

            return View(detail);
        }

        private string FormatDate(string detailsDate)
        {
            DateTime date = DateTime.Parse(detailsDate);
            var result = date.ToString("d/MMMM/yyyy").Replace("/"," ");

            return (result);
        }

        private string FormatReg(string registration)
        {
            var result = registration.Insert(4, " ");
            return result;
        }


        private bool VehicleRegEx(string registration)
        {
            var regexPattern = @"/^([A-Z]{3}\s?(\d{3}|\d{2}|d{1})\s?[A-Z])|([A-Z]\s?(\d{3}|\d{2}|\d{1})\s?[A-Z]{3})|(([A-HK-PRSVWY][A-HJ-PR-Y])\s?([0][2-9]|[1-9][0-9])\s?[A-HJ-PR-Z]{3})$/";
            var regExIsValid = Regex.IsMatch(registration, regexPattern);

            return regExIsValid;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}