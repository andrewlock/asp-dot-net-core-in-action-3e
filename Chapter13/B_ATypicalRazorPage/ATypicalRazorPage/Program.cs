using ATypicalRazorPage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ToDoService>();

var app = builder.Build();

app.MapRazorPages();

// When viewing in the browser, the default page ('/') is requested
// But as that page does not exist, redirect to a page that does!
// You can also try /category/Long 
app.Map("/", () => Results.Redirect("/category/Simple"));
app.Run();
