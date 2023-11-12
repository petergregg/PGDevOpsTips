namespace PGDevOpsTips.Workflow.Interfaces
{
    /// <summary>
    /// Service for working with markdown.
    /// </summary>
    public interface IMarkdownService
    {
        /// <summary>
        /// Converts Markdown to a html string
        /// </summary>
        /// <param name="markdown">The Markdown.</param>
        /// <returns>Returns a html string</returns>
        string MarkdownToHtml(string markdown);
    }
}
