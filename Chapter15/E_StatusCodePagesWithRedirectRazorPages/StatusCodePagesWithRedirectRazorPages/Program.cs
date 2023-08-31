var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

//Note that using this approach we're ultimately returning a success code
//when an error actually occured. 
//Reference: https://andrewlock.net/re-execute-the-middleware-pipeline-with-the-statuscodepages-middleware-to-create-custom-error-pages/
app.UseStatusCodePagesWithRedirects("/error/{0}");

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();