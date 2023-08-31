﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Pages.ToDo
{
    public class ListCategoryModel : PageModel
    {
        private readonly ToDoService _service;

        public ListCategoryModel(ToDoService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        public IEnumerable<Task> Tasks { get; set; }

        public IActionResult OnGet()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            //TODO: Validate the parameters
            Tasks = _service.GetToDoItems(Input.Category, Input.Username)
                .Select(x => new Task(x.Number, x.Title));

            return Page();
        }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Category { get; set; }
        }

        public class Task
        {
            public Task(int id, string description)
            {
                Id = id;
                Description = description;
            }

            public int Id { get; }

            public string Description { get; set; }
        }
    }
}