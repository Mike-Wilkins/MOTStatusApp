using CustomerApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Interfaces;

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


            _viewData.mOTStatusDetails.DateOfRegistration = FormatDate(_viewData.mOTStatusDetails.DateOfRegistration);


            _viewData.mOTStatusDetails.DateOfLastMOT = FormatDate(_viewData.mOTStatusDetails.DateOfLastMOT);

            return View(_viewData);
        }

        private string FormatDate(string detailsDate)
        {
            DateTime date = DateTime.Parse(detailsDate);
            var result = date.ToString("d/MMMM/yyyy").Replace("/", " ");

            return (result);
        }
    }
}
