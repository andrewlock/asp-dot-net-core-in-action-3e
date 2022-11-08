using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RecipeService _service;
        private readonly ILogger<IndexModel> _logger;
        public ICollection<RecipeSummaryViewModel> Recipes { get; private set; }

        public IndexModel(RecipeService service, ILogger<IndexModel> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task OnGet()
        {
            Recipes = await _service.GetRecipes();
            _logger.LogInformation("Loaded {RecipeCount} recipes", Recipes.Count);
        }
    }
}
