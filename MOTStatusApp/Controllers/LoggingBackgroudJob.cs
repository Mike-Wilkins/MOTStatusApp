
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using Quartz;


namespace MOTStatusWebApi.Controllers
{
    public class LoggingBackgroudJob : IJob
    {
        private readonly ILogger<LoggingBackgroudJob> _logger;

        private readonly IMOTStatusDetailsRepository _statusDetailsRepository;

        public LoggingBackgroudJob(ILogger<LoggingBackgroudJob> logger, IMOTStatusDetailsRepository statusDetailsRepository)
        {
            _logger = logger;
            _statusDetailsRepository = statusDetailsRepository;
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

        public static MOTStatusDetails FormatObjectDetails(MOTStatusDetails details)
        {
            details.TaxDueDate = UpdateVehicleDueDate(details.DateOfLastV5C);
            details.MOTDueDate = UpdateVehicleDueDate(details.DateOfLastMOT);

            details.Taxed = IsVehicleTaxedAndMOTed(details.TaxDueDate);
            details.MOTed = IsVehicleTaxedAndMOTed(details.MOTDueDate);

            if (details.Taxed)
            {
                details.VehicleStatus = "Taxed";
            }
            else { details.VehicleStatus = "Not Taxed"; }

            return details;
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
    }
}
