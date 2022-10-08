var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ToDoService>();

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

public record ToDoItem(int Id, params string[] Tasks)
{
    public bool IsComplete => Tasks.Length == 0;
}

public class ToDoService
{
    public List<ToDoItem> AllItems { get; } = new List<ToDoItem>
    {
        new ToDoItem(1, "Buy milk", "Buy eggs", "Buy bread"),
        new ToDoItem(2, "Pick up kids", "Take kids to school"),
        new ToDoItem(3, "Get fuel", "<strong>Check oil</strong>", "Check Tyre pressure"),
        new ToDoItem(4, "Get fuel", "Check oil", "Check Tyre pressure"),
    };
}