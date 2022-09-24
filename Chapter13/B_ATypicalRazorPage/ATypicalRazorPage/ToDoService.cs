namespace ATypicalRazorPage;
public record ToDoListModel(string Category, string Title);

public class ToDoService
{
    // These would normally be loaded from a database for example
    private static readonly List<ToDoListModel> _items = new List<ToDoListModel>
        {
            new ToDoListModel("Simple", "Bread"),
            new ToDoListModel("Simple", "Milk"),
            new ToDoListModel("Simple", "Get Gas"),
            new ToDoListModel("Long", "Write Book"),
            new ToDoListModel("Long", "Build Application"),
        };

    public List<ToDoListModel> GetItemsForCategory(string category)
    {
        // filter by the provided category
        return _items.Where(x => x.Category == category).ToList();
    }
}