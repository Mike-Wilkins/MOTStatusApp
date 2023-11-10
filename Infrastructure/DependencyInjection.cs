using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = JobKey.Create(nameof(LoggingBackgroudJob));

                options
                    .AddJob<LoggingBackgroudJob>(jobKey)
                    .AddTrigger(trigger =>
                        trigger
                            .ForJob(jobKey)
                            .WithSimpleSchedule(schedule =>
                                schedule.WithIntervalInSeconds(60).RepeatForever()));


            });

            services.AddQuartzHostedService();
        }

    }
}