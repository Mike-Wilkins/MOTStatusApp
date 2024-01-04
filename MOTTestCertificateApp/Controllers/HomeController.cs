using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Interfaces;
using MOTStatusWebApi.Models;
using MOTTestCertificateApp.Interfaces;
using MOTTestCertificateApp.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MOTTestCertificateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMOTTestCertificateDetailsRepository _testDetailsRepository;
        private readonly IMOTStatusDetailsRepository _statusDetailsRepository;
        private readonly IMOTTestCertificateViewData _viewData;

        public HomeController(ILogger<HomeController> logger, IMOTTestCertificateDetailsRepository testDetailsRepository, IMOTTestCertificateViewData viewData, IMOTStatusDetailsRepository statusDetailsRepository)
        {
            _logger = logger;
            _testDetailsRepository = testDetailsRepository;
            _viewData = viewData;
            _statusDetailsRepository = statusDetailsRepository;
        }

        public IActionResult Index()
        {
            return View(_viewData);
        }

        [HttpPost]
        public IActionResult Index(string registration)
        {
            if (registration == null)
            {
                _viewData.RegistrationValidationError = true;
                return View(_viewData);
            }

            registration = registration.ToUpper().Replace(" ", "");

            var regexValidation = VehicleRegEx(registration);

            if (!regexValidation)
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

            return RedirectToAction("Create", new { registration = registration });
        }

        public IActionResult Create(string registration)
        {
            var details = _statusDetailsRepository.GetStatusDetails().
                Where(d => d.RegistrationNumber == registration.ToUpper()).FirstOrDefault();

            var testDetailsCount = (_testDetailsRepository.GetTestCertificateDetails().Count())+1;
            var certificateDetails = new MOTTestCertificateDetails();
            certificateDetails.RegistrationNumber = details.RegistrationNumber;
            certificateDetails.FuelType = details.FuelType;
            certificateDetails.Colour = details.VehicleColour;
            certificateDetails.DateOfRegistration = details.DateOfRegistration;
            certificateDetails.Make = details.Make;
            certificateDetails.MOTTestNumber = testDetailsCount.ToString();
            certificateDetails.DateOfLastMOT = DateTime.Now.ToString("g");
            certificateDetails.Model = details.Model;
            certificateDetails.VehicleID = details.VehicleID;
            certificateDetails.MOTDueDate = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");

            return View(certificateDetails);
        }

        [HttpPost]
        public IActionResult Create(MOTTestCertificateDetails testCertificateData)
        {

            if (!ModelState.IsValid)
            {
                return View(testCertificateData);
            }

            _testDetailsRepository.Add(testCertificateData);

            return View(testCertificateData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public static bool VehicleRegEx(string registration)
        {
            var regExIsValid = Regex.IsMatch(registration, @"^(?=.{1,7})(([a-zA-Z]?){1,3}(\d){1,4}([a-zA-Z]?){1,3})$");

            return regExIsValid;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}