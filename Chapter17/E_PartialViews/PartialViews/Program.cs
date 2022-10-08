var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<TaskService>();

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

public record ToDoItemViewModel(int Id, string Title, params string[] Tasks)
{
    public bool IsComplete => Tasks.Length == 0;
}

public class TaskService
{
    //This list would be loaded from a database or file etc
    public List<ToDoItemViewModel> AllTasks { get; } = new List<ToDoItemViewModel>
        {
            new ToDoItemViewModel(1, "Shopping List", "Buy milk", "Buy eggs", "Buy bread"),
            new ToDoItemViewModel(2, "Agenda", "Pick up kids", "Take kids to school"),
            new ToDoItemViewModel(4, "Car stuff", "Get fuel", "Check oil", "Check Tyre pressure"),
            new ToDoItemViewModel(4, "Problems"),
            new ToDoItemViewModel(5, "Writing tasks","Write blog post", "Edit book chapter"),
        };

}