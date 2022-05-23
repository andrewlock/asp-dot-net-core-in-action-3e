var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Note that this is for demonstration only
// Using a List is not thread-safe and should not be used in 
// practice. Instead, use a database or a thread-safe data structure
var _people = new List<Person>
{
    new("Tom", "Hanks"),
    new("Denzel", "Washington"),
    new("Leondardo", "DiCaprio"),
    new("Al", "Pacino"),
    new("Morgan", "Freeman"),
};

// This example ignores the case for the name using OrdinalIgnoreCase
app.MapGet("/person/{name}", (string name) =>
    _people.Where(p => p.FirstName.StartsWith(name, StringComparison.OrdinalIgnoreCase)));

app.Run();

record Person(string FirstName, string LastName);