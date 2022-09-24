namespace ConvertingToMVC;

public class CategoryViewModel
{
    public List<ToDoListModel> Items { get; set; }

    public CategoryViewModel(List<ToDoListModel> items)
    {
        Items = items;
    }
}
