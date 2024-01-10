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

        public IActionResult MOTHistoryIndex(string vehicleId, string dateOfRegistration, string dateOfLastMOT)
        {
            _viewData.mOTTestCertificateDetails = _testDetailsRepository.GetTestCertificateDetails().Where(d => d.VehicleID == vehicleId).ToList();

            _viewData.mOTStatusDetails = _statusDetailsRepository.GetStatusDetails().Where(d => d.VehicleID == vehicleId).FirstOrDefault();

            _viewData.mOTStatusDetails.DateOfRegistration = dateOfRegistration;
            _viewData.mOTStatusDetails.DateOfLastMOT = dateOfLastMOT;

            foreach (var item in _viewData.mOTTestCertificateDetails)
            {
                item.DateOfLastMOT = FormatDate(item.DateOfLastMOT);
                item.MOTDueDate = FormatDate(item.MOTDueDate);

                var mileage = Int32.Parse(item.OdometerReading);
                item.OdometerReading = String.Format("{0:#,##0.##}", mileage);
            }



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
