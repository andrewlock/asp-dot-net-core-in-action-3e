using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CustomTagHelpers;

public class MarkdownTagHelper: TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var markdownRazorContent = await output.GetChildContentAsync(NullHtmlEncoder.Default);
        var markdown = markdownRazorContent.GetContent(NullHtmlEncoder.Default);
        var html = Markdig.Markdown.ToHtml(markdown);

        output.Content.SetHtmlContent(html);
        output.TagName = null;
    }
}
