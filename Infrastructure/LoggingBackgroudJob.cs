using Microsoft.Extensions.Logging;
//using MOTStatusWebApi.Interfaces;

using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class LoggingBackgroudJob : IJob
    {
        private readonly ILogger<LoggingBackgroudJob> _logger;

        //private readonly IMOTStatusDetailsRepository _statusDetailsRepository;

        public LoggingBackgroudJob(ILogger<LoggingBackgroudJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{UtcNow}", DateTime.UtcNow);

            return Task.CompletedTask;

        }
    }
}
