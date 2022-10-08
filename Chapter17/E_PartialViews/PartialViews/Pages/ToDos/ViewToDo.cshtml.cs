using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PartialViews.Pages.ToDos
{
    public class ViewToDoModel : PageModel
    {
        private readonly TaskService _taskService;

        public ViewToDoModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public ToDoItemViewModel Item { get; set; }

        public IActionResult OnGet(int id)
        {
            Item = _taskService.AllTasks.FirstOrDefault(x => x.Id == id);
            if (Item == null)
            {
                return RedirectToPage("RecentToDos");
            }
            return Page();
        }
    }
}
