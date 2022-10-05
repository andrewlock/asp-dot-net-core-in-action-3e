var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/{0}");
app.MapGet("/", () => "Try navigating to /missing to see the error page");
app.MapGet("/404", () => "Oops! We couldn't find the page you requested. Please check the url you entered and try again");

app.Run();