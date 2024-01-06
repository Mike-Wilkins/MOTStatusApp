using CustomerApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using MOTStatusWebApi.Models;
using Newtonsoft.Json;

namespace CustomerApp.Controllers
{
    public class MOTHistoryController : Controller
    {
        private readonly IMOTCustomerStatusViewData _viewData;
        private readonly IMOTTestCertificateDetailsRepository _testDetailsRepository;
        private readonly IMOTStatusDetailsRepository _statusDetailsRepository;

        public MOTHistoryController(IMOTCustomerStatusViewData viewData, IMOTTestCertificateDetailsRepository testDetailsRepository, IMOTStatusDetailsRepository statusDetailsRepository)
        {
           _viewData = viewData;
            _testDetailsRepository = testDetailsRepository;
            _statusDetailsRepository = statusDetailsRepository;
        }

        public IActionResult MOTHistoryIndex(string vehicleId)
        {   
            _viewData.mOTTestCertificateDetails = _testDetailsRepository.GetTestCertificateDetails().Where(d => d.VehicleID == vehicleId).ToList();
            _viewData.mOTStatusDetails = _statusDetailsRepository.GetStatusDetails().Where(d => d.VehicleID == vehicleId).FirstOrDefault();

            return View(_viewData);
        }
    }
}
