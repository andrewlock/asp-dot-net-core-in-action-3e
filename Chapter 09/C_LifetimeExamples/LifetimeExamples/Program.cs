var builder = WebApplication.CreateBuilder(args);
builder.Host.UseDefaultServiceProvider(o =>
{
    // Set the below values to true to always validate scopes,
    // These are only set to false here to demonstrate the (errorenous)
    // behaviour of captured dependencies on the /captured endpoint
    o.ValidateScopes = false;
    o.ValidateOnBuild = false;

    // The default definition (commented out below) only
    // validates in dev environments (for performance reasons)
    //o.ValidateScopes = builder.Environment.IsDevelopment();
    //o.ValidateOnBuild = builder.Environment.IsDevelopment();
});


builder.Services.AddTransient<TransientRepository>();
builder.Services.AddTransient<TransientDataContext>();

builder.Services.AddScoped<ScopedRepository>();
builder.Services.AddScoped<ScopedDataContext>();

builder.Services.AddSingleton<SingletonRepository>();
builder.Services.AddSingleton<SingletonDataContext>();
builder.Services.AddSingleton<CapturingRepository>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ScopedDataContext>();
    Console.WriteLine($"Retrieved scope with RowCount: {dbContext.RowCount}");
}

app.MapGet("/", () => @"Visit /singleton, /scoped, /transient, or /captured.

Refresh the page a few times to see the relationship between the 
DataContext values and the Repository values, and how these
change when refreshing the page
");

List<string> _singletons = new();
List<string> _scopeds = new();
List<string> _transients = new();
List<string> _captured = new();

app.MapGet("/singleton", Singleton);
app.MapGet("/scoped", Scoped);
app.MapGet("/transient", Transient);
app.MapGet("/captured", Captured);



string Singleton(SingletonDataContext db, SingletonRepository repo) 
    => RowCounts(db, repo, _singletons);

string Scoped(ScopedDataContext db, ScopedRepository repo)
    => RowCounts(db, repo, _scopeds);

string Transient(TransientDataContext db, TransientRepository repo)
    => RowCounts(db, repo, _transients);

string Captured(ScopedDataContext db, CapturingRepository repo)
    => RowCounts(db, repo, _captured);

static string RowCounts(DataContext db, Repository repo, List<string> previous)
{
    var counts = $"{db.GetType().Name}: {db.RowCount:000,000,000}, {repo.GetType().Name}: {repo.RowCount:000,000,000}";

    var result = $@"
Current values:
{counts}

Previous values:
{string.Join(Environment.NewLine, previous)}";
    
    previous.Insert(0, counts);
    return result;
}


app.Run();

class DataContext
{
    // The class will return the same random number for its lifetime
    public int RowCount { get; } = Random.Shared.Next(1, 1_000_000_000);
}

class TransientDataContext : DataContext { }
class ScopedDataContext : DataContext { }
class SingletonDataContext : DataContext { }

class Repository
{
    private readonly DataContext _dataContext;
    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public int RowCount => _dataContext.RowCount;
}

class ScopedRepository : Repository
{
    public ScopedRepository(ScopedDataContext generator) : base(generator) { }
}

class TransientRepository : Repository
{
    public TransientRepository(TransientDataContext generator) : base(generator) { }
}

class SingletonRepository : Repository
{
    public SingletonRepository(SingletonDataContext generator) : base(generator) { }
}

class CapturingRepository : Repository
{
    public CapturingRepository(ScopedDataContext generator) : base(generator) { }
}