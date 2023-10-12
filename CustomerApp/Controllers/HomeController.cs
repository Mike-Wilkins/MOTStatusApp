using CustomerApp.Models;
using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Data;
using Newtonsoft.Json;
using System.Diagnostics;

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
            var carDetail = JsonConvert.DeserializeObject<MOTStatusDetails>(await client.GetStringAsync(url + registration));

            return View(carDetail);
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