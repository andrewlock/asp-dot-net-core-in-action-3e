var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDeveloperExceptionPage(); // can be omitted, added by WebApplication by default
app.UseStaticFiles();
app.UseRouting();

app.MapGet("/", () => "Hello World!");

app.Run();
