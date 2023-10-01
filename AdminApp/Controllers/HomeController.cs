using AdminApp.Models;
using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Interfaces;
using System.Diagnostics;
using System.Linq;

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
            return View();
        }

        [HttpPost]
        public IActionResult Index(string registration)
        {
            var details = _statusDetailsRepository.GetStatusDetails().Where(d => d.RegistrationNumber == registration).FirstOrDefault();
            return View();
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