using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CustomTagHelpers;

public class MarkdownTagHelper: TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var markdownRazorContent = await output.GetChildContentAsync();
        var markdown = markdownRazorContent.GetContent();
        var html = Markdig.Markdown.ToHtml(markdown);

        output.Content.SetHtmlContent(html);
        output.TagName = null;
    }
}
