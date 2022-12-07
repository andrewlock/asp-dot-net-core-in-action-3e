using Lamar;
using Lamar.Microsoft.DependencyInjection;
using LamarExample;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLamar(services =>
{
    // You can add standard ASP.Net Core DI abstractions
    services.AddAuthorization();

    // Exposes Lamar specific registrations
    // and functionality
    services.Scan(s =>
    {
        // Automatically register services that follow default conventions, e.g.
        // PurchasingService/IPurchasingService, Concrete types like ConcreteService
        // Typically, you will have a lot of Service/IService pairs in your app
        s.AssemblyContainingType(typeof(Program));
        s.WithDefaultConventions();

        // Register all of the implementations of IGamingService
        // CrosswordService
        // SudokuService
        s.AddAllTypesOf<IGamingService>();
        // Register all non-generic implementations of IValidatior<T> (UserModelValidator)
        s.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
    });

    // When a ILeaderboard<T> is requested, use Leaderboard<T>
    // Equivalent to:
    // services.AddTransient(typeof(ILeaderboard<>), typeof(Leaderboard<>));
    services.For(typeof(ILeaderboard<>)).Use(typeof(Leaderboard<>));
    // When an IUnitOfWork<T> is requested, run the lambda
    // Also, has a "scoped" lifetime, instead of the default "transient" lifetime
    // Equivalent to:
    //services.AddScoped<IUnitOfWork>(_ => new UnitOfWork(3));
    services.For<IUnitOfWork>().Use(_ => new UnitOfWork(3)).Scoped();
    // For a given T, when an IValidator<T> is requested, 
    // but there are no non-generic implementations of IValidator<T>
    // Use DefaultValidator<T> instead
    // No equivalent using the built-in container
    services.For(typeof(IValidator<>)).Add(typeof(DefaultValidator<>));
});

var app = builder.Build();

// var container = (IContainer)app.Services;
// Console.WriteLine(container.WhatDidIScan());
// Console.WriteLine(container.WhatDoIHave());

app.MapGet("/", (
    IEnumerable<IGamingService> gamingServices,
    IPurchasingService purchasingService,
    IUnitOfWork unitOfWork,
    ILeaderboard<UserModel> leaderboard,
    ConcreteService concreteService) => "Hello!");

app.Run();
