Book project for ASP.NET Core in Action, Third Edition
==============================
This repository contains the code samples for *ASP.NET Core in Action, Third Edition*

> **Warning**
> For clarity, this repository uses project names with long names. 
> This can sometimes cause problems building the samples on Windows if you checkout the repository to a long path, with errors similar to
> ```bash
> Error MSB4018: The "GenerateStaticWebAsssetsPropsFile" task failed unexpectedly
> System.IO.DirectoryNotFoundException: Could not find a part of the path
> ...
> ```
>  Where possible, checkout the repository to a short path, e.g. `C:\repos\asp-dot-net-core-in-action-3e`. 
> 

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
* *MultipleVerbMinimalApiWithAutoProblemDetails* - 5.3.2 Returning useful errors with ProblemDetails
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

## [Chapter 14](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter14)
* *RoutingExamples* - Multiple examples of routing. In particular, see _Search.cshtml_,  _Products.cshtml_, and _ProductDetails/Index.cshtml_. _Index.cshtml_ includes links demonstrating route parameters
* *ChangingConventions* - Customizing the URLs using conventions. See _Program.cs_

## [Chapter 15](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter15)
* *PageHandlers* - 15.3 Using different Razor Page handlers to handle different HTTP verbs (`GET` and `POST`)
* *StatusCodePages* - 15.5 A basic minimal API application demonstrating the StatusCodePagesMiddleware
* *StatusCodePagesWithRexecute* - 15.5 Reexecuting the pipeline to create custom status code pages
* *StatusCodePagesWithReExecuteRazorPages* - 15.5 Reexecuting the pipeline to create custom status code pages in a Razor Pages application, with a custom 404 error page, and a generic error page for other errors.
* *StatusCodePagesWithRedirect* - 15.5 Redirecting the pipeline to create custom status code pages in a Razor Pages application, with a custom 404 error page, and a generic error page for other errors.

## [Chapter 16](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter16)
* *ToDoList* - Basic application demonstrating the application in Figure 16.1
* *ExampleBinding_EditProduct* - Binding a custom class using Model Binding
* *ExampleBinding_Calculator* - Binding a custom class using Model Binding
* *SimpleCurrencyConverterBindings* - Model binding simple properties. Demonstrates Table 16.1, binding to route parameters, form parameters and the querystring
* *ListBinding* - Model binding to collections, as shown in Figure 16.5
* *ValidatingWithDataAnnotations* - A dummy checkout page, demonstrating Model validation using various `DataAnnotations`. Also shows POST-REDIRECT-GET
* *CurrencyConverter* - A dummy currency converter application. Demonstrates model binding, `DataAnnotations`, and a custom validation attribute
* *RazorPageFormLayout* - Organising a Razor Page for model binding, as in section 16.4

## [Chapter 17](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter17)
* *ManageUsers* - Display a list of users, and allow adding new users, as shown in figure 17.3
* *DynamicHtml* - Example of creating dynamic HTML by using C# in Razor templates.
* *ToDoList* - Example of writing model values to HTML in Razor templates, as shown in section 17.2
* *NestedLayouts* - Demonstrates using a nested layout, where a two column layout, `_TwoColumn.cshtml` is nested in `_Layout.cshtml`.
* *PartialViews* - Demonstrates extracting common code into a partial view, as in section 17.4.3. Also shows adding additional namespace to _ViewImports for `PartialViews.Models` namespace.

## [Chapter 18](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter18)
* *CurrencyConverter* - A demo currency converter application using TagHelpers to generate form elements.
* *TagHelpers* - Demonstrates the input types generated for various property types and `DataAnnotations`, as described table 18.1.
* *SelectLists* - Generating a variety of select lists, as shown in section 18.2.4
* *EnvironmentTag* - Using the environment tag to conditionally render content, as shown in section 18.5

## [Chapter 19](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter19)
* *WebApplication1* - Your first MVC controller application. Created using the default Web App (Model-View-Controller) template.
* *FindingAView* - Default MVC template, showing the default conventions for finding a view. Also shows how you can specify the template to Render - the `HomeController` specifies alternative views to render at the URLs `/Home/IndexThatRendersPrivacy` and `/Home/IndexThatRendersPrivacyAbsolute`.

## [Chapter 20](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter20)
* *DefaultWebApiProject* - The default Web API project, created using the Visual Studio API template, as in section 20.2.
* *BasicWebApiProject* - A basic Web API project, returning a list of fruit, as demonstrated in section 20.2.
* *ApiControllerAttribute* - A project containing 2 controllers, demonstrating the additional code required if you don't use the `[ApiController]` attribute, as in section 20.5.
* *ProblemDetailsExample* - A simple API controller that demonstrates automatically returning a `ValidationProblemDetails` object when the binding model (the `myValue` route parameter) is empty.
* *CarsWebApi* - A Web API controller that demonstrates generating various different response types. Is configured to allow XML output in Program.cs Use Swagger UI to make requests to the API and view the XML response. Also configured to use the _Newtonsoft.Json_ formatter instead of the _System.Text.Json_ formatter.

## [Chapter 22](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter22)
* *FilterPipelineExample* - A sample application with a single API Controller and a single Razor Page that contains one of each filter, and logs when the filter runs. Each filter contains commented out code to short-circuit the pipeline. Uncomment the code from each filter in turn to see the effect.
* *RecipeApplication* - The RecipeApplication from chapter 12 plus two API controllers. The `NoApiController` includes the code from listing 21.8, while the `RecipeApiController` includes the code from listing 20.9 where the code is refactored to use filters.


## [Chapter 22](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter22)
* *FilterPipelineExample* - A sample application with a single API Controller and a single Razor Page that contains one of each filter, and logs when the filter runs. Each filter contains commented out code to short-circuit the pipeline. Uncomment the code from each filter in turn to see the effect.
* *RecipeApplication* - The RecipeApplication from chapter 12 plus two API controllers. The `NoApiController` includes the code from listing 21.8, while the `RecipeApiController` includes the code from listing 20.9 where the code is refactored to use filters.

## [Chapter 23](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter14)
* *DefaultTemplate* - The default web app template for ASP.NET Core with Authentication, as discussed in section 23.3. Examples using LocalDB (created through Visual Studio) and using SQLite (created using `dotnet new webapp --auth Individual`)
* *RecipeApplication* - The starting point "existing" recipe application, as described in section 23.4.
* *RecipeApplication_WithIdentity* - The recipe application with authentication added, as described in section 23.4. Also, the register page has been scaffolded to remove the references to external services, as described in section 23.5.
* *RecipeApplication_WithNameClaim* - The recipe application with an additional field added to the `RegisterModel` to record the `FullName`, as described in section 23.6. The field is added as an extra claim when the user registers, and is displayed in the menu bar when a user logs in.

## [Chapter 24](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter24)
* *Airport* - An analogy to the airport example presented in section 24.1. There are 4 steps, Home Page, Through security, Enter airport lounge, Board the plane. You can set the claims for a user when you register. Which claims you add will determine how far through the airport you can get.
* *RecipeApplication* - The *RecipeApplication_WithIdentity* from chapter 23, with authorization to prevent unauthorized users creating recipes, and resource based authorization to ensure only the user which created a recipe can edit it.

## [Chapter 25](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter25)
* *RecipeApi_WithAuthentication* - The recipe API from chapter 12, with added authentication, as described in section 25.3.
* *RecipeApi_SwaggerAuth* - Extending the recipe API to describe the authorization requirement for OpenAPI and Swagger UI, as described in section 25.5.
* *RecipeApi_WithAuthorizationPolicies* - Adding additional authorization policies to the CreateRecipe API and management APIs, as described in section 25.6. An endpoint filter is used to apply resource-based authorization to multiple endpoints.

## [Chapter 26](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter26)
* *RecipeApplication* - The recipe application from previous chapters with some additional logging added to some Razor Pages.
* *FileLogger* - A simple minimal API Project configured to write log messages to a rolling file by using a rolling file logging provider, as shown in section 26.3. Note that the log levels have been changed from the defaults in appsettings.json and appsettings.Development.json to show more in the logs.
* *LogFiltering* - A simple minimal API Project configured to use the configuration filters defined in section 26.4.
* *SeqLogger* - A simple minimal API project to demonstrate structured logging using Seq, and using scopes to add additional properties to a log, as shown in section 26.5.

## Chapter 27
*No code samples*

## [Chapter 28](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter28)
* *CustomHttpsCertificate* - A basic Razor Pages app using Kestrel with a self-signed certificate, similar to the way you would configure a certificate in production. Shows configuring the default certificate used by Kestrel in _appsettings.json_.

On Windows, you can generate a self-signed certificate using the *Install-Certificate.ps1* PowerShell script. This will create a self-signed certificate and add it to Windows' trusted certificate store. You must run this from an elevated command prompt.

You can generate a certificate on Ubuntu using *install_certificate.sh*. This uses *localhost.conf* to create a self signed certificate, and trusts it. On Linux, not all applications use the same store, so you may have to trust it explicitly for those applications. Use password `testpassword` to create the certificate.

## [Chapter 29](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter29)
* *CrossSiteScripting* - A simple app to demonstrate XSS attacks due to not encoding user input. The user submits content which is added to an internal list and is later rendered to the page. Using `@Html.Raw` renders the provided input exactly as it was entered - if the content is malicious, e.g. a `<script>` tag, then it is written into the page as a script tag, executing any code it contains. Instead, you should render content with the `@` symbol alone - that way the content is rendered as a string, and can be displayed safely.

* *CrossSiteRequestForgery* - A pair of apps to demonstrate a CSRF vulnerability. You can login to the banking application and view your balance. You can 'withdraw' funds using the provided form, and you'll see your balance reduce. The attacker website contains a form that posts to the banking application and withdraws funds for the currently logged in user. In the example you have to click the button to see the vulnerability, but this could easily be automated. To protect the endpoint, add the `[ValidateAntiForgeryToken]` attribute to the `BalanceController.Withdraw()` action. Run both applications by selecting "Set Startup Projects" in Visual Studio, or by running both applications using `dotnet run`.

* *CorsApplication* - A pair of apps to demonstrate CORS. The "shopping.com" site is a Razor Pages application, that loads a product list from a separate app, "api.shopping.com", hosted at a different host. With the default configuration, the request succeeds. Experiment by removing the default CORS policy from the `UseCors()` middleware configuration, and applying `[EnableCors]` to the `ProductsController` instead. Note that it's the _API_ that defines which applications can call it. Run both applications by selecting "Set Startup Projects" in Visual Studio, or by running both applications using `dotnet run`.

## [Chapter 30](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter30)
* *RecipeApplication* - The recipe application from previous chapters, converted to use the generic host, with a `Startup` class.

## [Chapter 31](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter31)
* *CustomMiddleware* - Various custom middleware, using, `Map`, `Run`, `Use`, and middleware classes, as described in section 31.1. Also, the `PingPongMiddleware`, `VersionMiddleware`, and `CalculatorMiddleware`, from the *CustomMiddleware* project, exposed as endpoints using _endpoint routing_.
* *ConfigureOptionsExample* - Configuring `IOptions` using services as described in section 31.2. Shows configuration using `OptionsBuilder` and using the alternative approach shown previously. The `IOptions<T>` are configured in multiple ways - from configuration values, from static values (using a Lambda), and using a service from DI. 
* *LamarExample* - Replacing the default DI container with Lamar (the successor to StructureMap), as in section 31.3. Demonstrates some of the functionality available in Lamar.

## [Chapter 32](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter32)
* *CustomTagHelpers* - Creating custom Tag Helpers, an `IfTagHelper` and `SystemInfoTagHelper`, as shown in section 32.1.
* *RecipeApplication* - The Recipe Application from previous chapters, this time with a custom view component, as described in section 32.2.
* *CurrencyConverter* - The demo Currency converter application, containing a custom validation attribute for validating the selected currencies, as in section 32.3.
* *FluentValidationConverter* - The demo Currency converter application, configured to use the FluentValidation library instead of DataAnnotations. Contains validation extension methods for validating the selected currencies, as in section 32.4.

## [Chapter 33](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter33)

* *SocketExhaustion* - A simple application that creates many `HttpClient`s, demonstrating sockets being consumed. Run `netstat` in a separate window, to view sockets stuck in the `TIME_WAIT` status, as discussed in section 33.1.1.
* *ExchangeRateViewer* - An API controller that calls a remote exchange rate API, and returns the value. Shows 4 different ways of using `HttpClient` and `IHttpClientFactory`:
  * Singleton `HttpClient`: A single `HttpClient` that lives for the life of the application, as discussed in section 33.1. This client won't respect DNS changes.
  * Using `IHttpClientFactory` to create an `HttpClient`, as described in section 33.2.1.
  * Using a _named_ `HttpClient`, as described in section 33.2.2
  * Creating a _typed_ `HttpClient`, as described in section 33.2.3
  * Adding transient error handling using _Polly_, as described in section 33.3
  * Creating a custom `HttpMessageHandler` for adding an API key, as described in section 33.4

## [Chapter 34](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter34)
* *BackgroundServiceCache* - An `IHostedService` that downloads exchange rates from a remote API and saves them in a dictionary, for consumption by an API controller, as described in section 34.1.1
* *BackgroundServiceDatabaseCache* - An `IHostedService` that uses scoped services, downloads exchange rates from a remote API and saves them in a dictionary, as described in section 34.1.2
* *SystemdService* - A generic `Host` to download exchange rates, configured to execute as a systemd daemon, as described in section 34.2
* *WindowsService* - A generic `Host` to download exchange rates, configured to execute as a Windows Service, as described in section 34.2.
* *QuartzHostedService* - A generic `Host` that uses Quartz.NET to run background tasks
* *QuartzClustering* - A generic `Host` that uses Quartz.NET to run background tasks configured to use clustering. Note that SQLite is not supported for clustering, so this application uses LocalDB

## [Chapter 35](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter35)
* *ExchangeRates* - A basic exchange rate application. Includes unit tests for the `CurrencyConverter` class.

## [Chapter 36](https://github.com/andrewlock/asp-dot-net-core-in-action-3e/tree/main/Chapter36)
* *ExchangeRates* - The basic exchange rate application from 35.2. Includes unit tests for the `CurrencyConverter` class (chapter 35), for the `StatusMiddleware` (section 36.1), for API controllers and minimal API endpoints (section 36.2). It also includes "Test Host" integration tests for the `StatusMiddleware` (section 36.3.1) as well as `WebApplicationFactory`-based integration tests for the whole app (section 36.3.2, 36.3.3, 36.3.4).
* *RecipeApplication* - Testing a service that relies on an EF Core `DbContext`, as described in section 36.4. The `RecipeServiceTests` class shows how you can test the `RecipeService` using the in-memory SQLite provider. Also shows a custom `WebApplicationFactory` implementation that uses an in-memory database.
