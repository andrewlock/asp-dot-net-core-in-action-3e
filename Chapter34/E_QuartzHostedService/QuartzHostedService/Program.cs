using QuartzHostedService.Data;
using QuartzHostedService;
using Microsoft.EntityFrameworkCore;
using Quartz;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(hostContext.Configuration.GetConnectionString("SqlLiteConnection")));

        services.AddHttpClient<ExchangeRatesClient>();

        services.AddQuartz(q =>
        {
            // Normally would take this from appsettings.json, just show it's possible
            q.SchedulerName = "Example Quartz Scheduler";

            // Use the DI container for creating IJobs
            q.UseMicrosoftDependencyInjectionJobFactory();

            // add the job
            var jobKey = new JobKey("Update exchange rates");
            q.AddJob<UpdateExchangeRatesJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithInterval(TimeSpan.FromMinutes(5))
                    .RepeatForever())
            );
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    })
    .UseSystemd()
    .Build();

host.Run();
