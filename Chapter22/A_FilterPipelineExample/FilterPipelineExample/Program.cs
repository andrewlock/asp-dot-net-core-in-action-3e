using FilterPipelineExample.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
    // .AddMvcOptions(options =>
    // {
        // Only add here OR under Controllers
        // options.Filters.Add(new GlobalLogAsyncActionFilter()); // won't apply to page
        // options.Filters.Add(new GlobalLogAsyncPageFilter()); // won't apply to action
        // options.Filters.Add(new GlobalLogAsyncAuthorizationFilter());
        // options.Filters.Add(new GlobalLogAsyncExceptionFilter());
        // options.Filters.Add(new GlobalLogAsyncResourceFilter());
        // options.Filters.Add(new GlobalLogAsyncResultFilter());
        // options.Filters.Add(new GlobalLogAsyncAlwaysRunResultFilter());
    // }); 

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new GlobalLogAsyncActionFilter()); // won't apply to page
    options.Filters.Add(new GlobalLogAsyncPageFilter()); // won't apply to action
    options.Filters.Add(new GlobalLogAsyncAuthorizationFilter());
    options.Filters.Add(new GlobalLogAsyncExceptionFilter());
    options.Filters.Add(new GlobalLogAsyncResourceFilter());
    options.Filters.Add(new GlobalLogAsyncResultFilter());
    options.Filters.Add(new GlobalLogAsyncAlwaysRunResultFilter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
