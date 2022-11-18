using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidationConverter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

ValidatorOptions.Global.LanguageManager.Enabled = false; // disables localization
builder.Services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true)
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton<ICurrencyProvider, CurrencyProvider>();

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
