var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<FruitService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public class FruitService
{
    public List<string> Fruit { get; } = new List<string>
    {
        "Pear",
        "Lemon",
        "Peach"
    };
}