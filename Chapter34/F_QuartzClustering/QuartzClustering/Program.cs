using QuartzClustering.Data;
using QuartzClustering;
using Microsoft.EntityFrameworkCore;
using Quartz;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection")!;

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddHttpClient<ExchangeRatesClient>();

        services.AddQuartz(q =>
        {
            // Normally would take this from appsettings.json, just show it's possible
            q.SchedulerName = "Example Quartz Scheduler";
            q.SchedulerId = "AUTO";

            // Use a Scoped container for creating IJobs
            q.UseMicrosoftDependencyInjectionJobFactory();

            q.UsePersistentStore(s =>
            {
                s.UseSqlServer(connectionString);
                s.UseClustering();
                s.UseProperties = true;
                s.UseJsonSerializer();
            });

            // add the job
            var jobKey = new JobKey("Update_exchange_rates");

            q.AddJob<UpdateExchangeRatesJob>(opts => opts.WithIdentity(jobKey));

            // Run every day, at 3am
            //q.AddTrigger(opts => opts
            //    .ForJob(jobKey)
            //    .WithIdentity(jobKey.Name + "_trigger")
            //    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour: 3, 0))
            //);

            // Run every 1 minute
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobKey.Name + "_trigger") // Important, 
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever()
                )
            );
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    })
    .UseSystemd()
    .Build();

host.Run();
