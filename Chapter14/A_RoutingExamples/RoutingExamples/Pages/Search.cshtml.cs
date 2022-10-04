using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#nullable disable

namespace RoutingExamples.Pages;

public class SearchModel : PageModel
{
    private readonly ProductService _productService;
    private readonly LinkGenerator _link;

    public SearchModel(ProductService productService, LinkGenerator link)
    {
        _productService = productService;
        _link = link;
    }

    [BindProperty, Required]
    public string SearchTerm { get; set; }
    public List<Product> Results { get; private set; }

    public void OnGet()
    {
        // Demonstrates link generation 
        var url1 = Url.Page("ProductDetails/Index", new { name = "big-widget" });
        var url2= _link.GetPathByPage("/ProductDetails/Index", values: new { name = "big-widget" });
        var url3 = _link.GetPathByPage(HttpContext, "/ProductDetails/Index", values: new { name = "big-widget" });
        var url4 = _link.GetUriByPage(
            page: "/ProductDetails/Index",
            handler: null,
            values: new { name = "big-widget" },
            scheme: "https",
            host: new HostString("example.com"));
    }

    public void OnPost()
    {
        if (ModelState.IsValid)
        {
            Results = _productService.Search(SearchTerm, StringComparison.Ordinal);
        }

    }
    public void OnPostIgnoreCase()
    {
        if (ModelState.IsValid)
        {
            Results = _productService.Search(SearchTerm, StringComparison.OrdinalIgnoreCase);
        }
    }
}