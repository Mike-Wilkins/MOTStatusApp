using CustomerApp.Interfaces;
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
        private readonly IMOTCustomerStatusViewData _viewData;

        HttpClient client = new HttpClient();
        string url = "https://localhost:7132/api/MOTStatusDetails/11/registration?registration=";
        public HomeController(ILogger<HomeController> logger, IMOTCustomerStatusViewData viewData)
        {
            _logger = logger;
            _viewData = viewData;
        }

        public IActionResult Index()
        {
            return View(_viewData); 
        }

        [HttpPost]
        public async Task<IActionResult> Index(string registration)
        {
            if (registration == null)
            {
                _viewData.RegistrationValidationError = true;          
                return View(_viewData);
            }

            registration = registration.Replace(" ", "");

            var regexValidation = VehicleRegEx(registration);

            if (!regexValidation)
            {
                _viewData.RegistrationFormatError = true;
                return View(_viewData);
            }

            try
            {
                var carDetail = JsonConvert.DeserializeObject<MOTStatusDetails>(await client.GetStringAsync(url + registration));             
                return RedirectToAction("DetailConfirmation", carDetail );
            }
            catch (Exception ex)
            {
                _viewData.RegistrationNotFoundError = true;
                return View(_viewData);
            }        
        }

        public IActionResult DetailConfirmation(MOTStatusDetails carDetails)
        {
            string formatReg = FormatReg(carDetails.RegistrationNumber);
            carDetails.RegistrationNumber = formatReg;
            _viewData.mOTStatusDetails = carDetails;
            return View(_viewData);
        }

        [HttpPost]
        public IActionResult DetailConfirmation(string confirm, MOTStatusDetails detail)
        {
            if(confirm == null)
            {
                string formatReg = FormatReg(detail.RegistrationNumber);
                detail.RegistrationNumber = formatReg;
                _viewData.mOTStatusDetails = detail;
                _viewData.ConfirmNotSelectedError = true;
                return View(_viewData);
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
            detail.DateOfRegistration = FormatDate(detail.DateOfRegistration);
            detail.DateOfLastMOT = FormatDate(detail.DateOfLastMOT);
            detail.DateOfLastV5C = FormatDate(detail.DateOfLastV5C);
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
            var regExIsValid = Regex.IsMatch(registration, @"^(?=.{1,7})(([a-zA-Z]?){1,3}(\d){1,4}([a-zA-Z]?){1,3})$");

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