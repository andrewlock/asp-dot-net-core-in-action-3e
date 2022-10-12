using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
                .AddRazorPagesOptions(opts =>
                {
                    // opts.Conventions.AddPageRouteModelConvention(new PrefixingPageRouteModelConvention()); // Add for all pages
                    opts.Conventions.AddPageRouteModelConvention("/Privacy", new PrefixingPageRouteModelConvention().Apply);
                    opts.Conventions.Add(new PageRouteTransformerConvention(new KebabCaseParameterTransformer()));
                    opts.Conventions.AddPageRoute("/ProductDetails/Search", "search-products");
                });

builder.Services.Configure<RouteOptions>(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

public class PrefixingPageRouteModelConvention : IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        var selectors = model.Selectors
            .Select(x => new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = AttributeRouteModel.CombineTemplates(
                        "page",
                        x.AttributeRouteModel!.Template),
                }
            })
            .ToList();

        foreach(var newSelector in selectors)
        {
            model.Selectors.Add(newSelector);
        }
    }
}


// .NET 6 version can't use [GeneratedRegex] attribute
//class KebabCaseParameterTransformer : IOutboundParameterTransformer
//{
//    public string? TransformOutbound(object? value)
//    {
//        if (value is null)
//        {
//            return null;
//        }

//        return Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();
//    }
//}

partial class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null)
        {
            return null;
        }

        return MyRegex().Replace(value.ToString(), "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}

