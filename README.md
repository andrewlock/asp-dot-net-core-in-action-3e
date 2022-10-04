Book project for ASP.NET Core in Action, Third Edition
==============================
This repository contains the code samples for *ASP.NET Core in Action, Third Edition*

## Chapter 1
*No code samples*

## Chapter 2
*No code samples*

## [Chapter 3](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter03)
* *WebApplication1* - A sample web application, based on the Visual Studio empty minimal API template.
* *WebApplication1Extended* - Section 3.7. A sample application, extending the empty minimal API template by adding services and middleware, and demonstrating JSON serialization.

## [Chapter 4](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter04)
* *CreatingAHoldingPage* - 4.2.1 Simple pipeline scenario 1: A holding page
* *CreatingAStaticFileWebsite* - 4.2.2 Simple pipeline scenario 2: Handling static files
* *SimpleMinimalApisApplication* - 4.2.3 Simple pipeline scenario 3: A minimal API application
* *MinimalApisAndWelcomePage* - 4.2.3 Simple pipeline scenario 3: A minimal API application + a holding page for "/"
* *DeveloperExceptionPage* - 4.3.1 Viewing exceptions in development: the DeveloperExceptionPage
* *ExceptionHandlerMiddleware* - 4.3.2 Handling exceptions in production: the ExceptionHandlerMiddleware

## [Chapter 5](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter05)
* *BlazorWebAssemblyProject* - A basic Blazor WebAssembly web application, as shown in Figure 5.2, created using the Blazor WebAssembly template. Follow the instructions at https://docs.microsoft.com/aspnet/core/blazor/get-started to try it for yourself.
* *BasicRoutingMinimalApi* - 5.2.1 Extracting values from the URL with routing
* *MultipleVerbMinimalApi* - 5.2.3 Defining route handlers with functions
* *MultipleVerbMinimalApiWithStatusCodes* - 5.3.1 Returning status codes with Results and TypedResults
* *MultipleVerbMinimalApiWithProblemDetails* - 5.3.2 Returning useful errors with ProblemDetails
* *MinimalApiWithAutoProblemDetails* - 5.3.3 Converting all your responses to Problem Details
* *MinimalApiFilters* - 5.4 Running common code with endpoint filters
* *MinimalApiRouteGroups* - 5.5 Organizing your APIs with route groups

## [Chapter 6](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter06)
* *RoutingExample* - 6.3 Exploring the route template syntax, 6.4 Generating URLs from route parameters
* *RoutingOptionsExample* - 6.4.3 Controlling your generated URLs with RouteOptions

## [Chapter 7](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter07)
* *BasicModelBinding* - Examples of binding throughout chapter 7
* *ValidatingWithDataAnnotations* - 7.10 Handling user input with model validation


## [Chapter 8](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter08)
* *SendingAnEmailWithoutDI* - An example demonstrating a use case where you want to send an email when a user registers. The `EmailSender` class is created in code using `new` as shown in section 8.1.
* *SendingAnEmailWithDI* - A refactoring of the *SendingAnEmailWithoutDI* project to use DI, showing how the `RegisterUser` endpoint handler has been simplified.

## [Chapter 9](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter09)
* *SendingAnEmailWithDI* - 9.1 Registering custom services with the DI container
* *InjectingMultipleImplementations* - Example demonstrating the behaviour when registering multiple instances of a service, as in section 9.3. Call the two endpoints shown on the home page and observe the console output to see the effect of the DI configuration.
* *LifetimeExamples* - The effect of lifetime on DI. For details, see section 9.4 - the project broadly follows this outline, with slightly different naming to allow registering all the services in a single project.


## [Chapter 10](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter10)
* *ReplacingDefaultConfigProviders* - Demonstrating how you can replace the configuration providers added by `WebApplicationBuilder`, as shown in section 10.2.1.
* *StoreViewerApplication* - A simple application that uses `IOptions<>` and strongly typed settings to bind configuration to POCOs. Optionally uses Google Maps to demonstrate loading settings from multiple sources.  Follow the [documentation from Google](https://developers.google.com/maps/documentation/javascript/get-api-key) to obtain an API key.
* *DesigningForAutomaticBinding* - Demonstrating how to create strongly typed settings that can be bound to configuration, and the limitations, as shown in section 10.3.3.
* *UsingDifferentEnvironments* - Demonstrates how to overwrite values based on the environment. In particular, observe how list values are overwritten.

## [Chapter 11](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter11)
* *OpenApiExample_Swashbuckle* - Adding an OpenAPI document to your app using Swashbuckle, as shown in sections 11.1, 11.2, and 11.3.
* *OpenApiExample_NSwag* - Adding an OpenAPI document to your app using NSwag. Equivalent to the Swashbuckle version, but using NSwag instead. 
* *GeneratingOpenApiClient* - Using NSwag to generate a C# client from an OpenAPI description, as shown in section 11.4.2. The API was added using the .NET OpenAPI tool.
* *CustomisingGeneration* - Customising the generated code to use System.Text.Json, no base URL, and to generate interfaces, as shown in section 11.4.3.
* *AddingDescriptions* - Adding summaries and descriptions to your endpoints using fluent methods, attributes, and XML documentation comments, as shown in section 11.5.

## [Chapter 12](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter12)
* *InstallEFCore* - Demonstrating how to install EF Core, as shown in section 12.2. Starts from the Default empty web template from Visual Studio and installs the EF Core packages, adds the `Recipe` and `Ingredient` entities, configures the `AppDbContext`, and registers EF Core with the DI container. Configured by default for Local DB, but demonstrates how to configure the application for SQLite instead.
* *Migrate_LocalDb* - The same code as *InstallEfCore*, but with migrations added using the `dotnet-ef` global tool. Migrations generated for Local DB.
* *Migrate_SQLite* - The same as *Migrate_LocalDb* but configured for SQLite, and with SQLite-specific migrations generated by the `dotnet-ef` global tool.
* *RecipeApplication_LocalDb* - The final minimal API recipe application, using the Local DB database provider.
* *RecipeApplication_SQLite* - The final minimal API recipe application, using the SQLite database provider.

## [Chapter 13](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter13)
* *WebApplication1* - Your first Razor Pages application. Created using the default Web App template.
* *ATypicalRazorPage* - 13.2.1 Exploring a typical Razor Page
* *ConvertingToMvc* - 13.3 An MVC application, with separate Model, View, and Controller files

## [Chapter 14](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter14)
* *RoutingExamples* - Multiple examples of routing. In particular, see _Search.cshtml_,  _Products.cshtml_, and _ProductDetails/Index.cshtml_. _Index.cshtml_ includes links demonstrating route parameters
* *ChangingConventions* - Customizing the URLs using conventions. See _Program.cs_