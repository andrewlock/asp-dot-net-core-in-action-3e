var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromHours(1);
});

var app = builder.Build();


if(app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();
