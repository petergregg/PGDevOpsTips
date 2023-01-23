using Markdig;
using PGDevOpsTips.Web.Interfaces;

namespace PGDevOpsTips.Web.Services
{
    public class MarkdownService : IMarkdownService
    {
        /// <inheritdoc/>
        public string MarkdownToHtml(string markdown)
        {
            // Parse markdown to html with MarkDig
            var mdPipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .UseAdvancedExtensions()
                .Build();
            var html = Markdown.ToHtml(markdown, mdPipeline);

            // Trim leading <H1> ... Bit hacky as it assumes that the H1 is the first line of html
            var htmlNoH1 = html.Substring(html.IndexOf("</h1>") + 5);

            htmlNoH1 = htmlNoH1.Replace("<table", "<table class='table'");

            // Return
            return htmlNoH1;
        }
    }
}
