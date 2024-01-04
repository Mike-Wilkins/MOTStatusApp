
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using Quartz;


namespace MOTStatusWebApi.Controllers
{
    public class LoggingBackgroudJob : IJob
    {
        private readonly ILogger<LoggingBackgroudJob> _logger;
        private readonly IMOTTestCertificateDetailsRepository _testDetailsRepository;
        private readonly IMOTStatusDetailsRepository _statusDetailsRepository;

        public LoggingBackgroudJob(ILogger<LoggingBackgroudJob> logger, IMOTStatusDetailsRepository statusDetailsRepository, IMOTTestCertificateDetailsRepository testDetailsRepository)
        {
            _logger = logger;
            _statusDetailsRepository = statusDetailsRepository;
            _testDetailsRepository = testDetailsRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{UtcNow}", DateTime.UtcNow);

             var details = _statusDetailsRepository.GetStatusDetails().ToList();

            foreach (var vehicle in details)
            {
                FormatObjectDetails(vehicle);
                _statusDetailsRepository.Update(vehicle);
            }

            return Task.CompletedTask;

        }

        public MOTStatusDetails FormatObjectDetails(MOTStatusDetails details)
        {
            details.TaxDueDate = UpdateVehicleDueDate(details.DateOfLastV5C, 1);

            DateTime registrastionDate = DateTime.Parse(details.DateOfRegistration);

            //Vehicles OLDER than 3 years
            if (DateTime.Now >= registrastionDate.AddYears(3))
            {
                var MOTList = _testDetailsRepository.GetTestCertificateDetails().Where(x => x.VehicleID == details.VehicleID).ToList();

                if (MOTList.Count == 0)
                {
                    details.MOTDueDate = registrastionDate.AddYears(3).ToString("dd/MM/yyyy");
                    details.DateOfLastMOT = details.DateOfRegistration;
                }
                else
                {
                    var latestMOTCert = MOTList.Max(m => m.MOTTestNumber);
                    var mOTDueDate = _testDetailsRepository.GetTestCertificateDetails().Where(d => d.MOTTestNumber == latestMOTCert).FirstOrDefault();
                    details.MOTDueDate = mOTDueDate.MOTDueDate;
                    details.DateOfLastMOT = mOTDueDate.DateOfLastMOT;
                }
            }

            //Vehicles LESS than 3 years old do not require MOT
            if (DateTime.Now <= registrastionDate.AddYears(3))
            {
                details.MOTDueDate = registrastionDate.AddYears(3).ToString();
                details.DateOfLastMOT = details.DateOfRegistration;
            }

            details.Taxed = IsVehicleTaxedAndMOTed(details.TaxDueDate); 
            details.MOTed = IsVehicleTaxedAndMOTed(details.MOTDueDate);
           

            if (details.Taxed)
            {
                details.VehicleStatus = "Taxed";
            }
            else { details.VehicleStatus = "Not Taxed"; }

            return details;
        }

        public static string UpdateVehicleDueDate(string date, int years)
        {
            DateTime currentDate = DateTime.Parse(date);
            DateTime dueDate = currentDate.AddYears(years);
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
    }
}
