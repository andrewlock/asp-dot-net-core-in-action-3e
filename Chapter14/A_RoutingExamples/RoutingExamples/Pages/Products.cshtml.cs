using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RoutingExamples.Pages;

#nullable disable

public class ProductsModel : PageModel
{
    private readonly ProductService _service;

    public ProductsModel(ProductService service)
    {
        _service = service;
    }

    [BindProperty(SupportsGet = true)]
    public string Name { get; set; }
    public Product Selected { get; set; }

    public IActionResult OnGet()
    {
        Selected = _service.GetProduct(Name);
        if(Selected is null)
        {
            return NotFound();
        }
        return Page();
    }

}