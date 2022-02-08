using System.IO;
using System.Text;
using PGDevOpsTips.Web.DataTransferObjects;
using PGDevOpsTips.Web.Interfaces;
using PGDevOpsTips.Web.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PGDevOpsTips.Web.Services
{
    /// <inheritdoc/>
    public class YamlService : IYamlService
    {
        public Content YamlToContent(string plainFileContents, Content content)
        {
            var yamlString = plainFileContents.Substring(0, plainFileContents.LastIndexOf("---\n")); // Chop off the markdown, leaving just the YAML header as YamlDotNet only deals with YAML documents. Assumes there is a space after the end of the YAML header

            var yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yamlHeader = yamlDeserializer.Deserialize<ContentYamlHeader>(yamlString);

            // Ensure we have a Title, Type and Status
            if (string.IsNullOrEmpty(yamlHeader.Title)
                || string.IsNullOrEmpty(yamlHeader.Type)
                || string.IsNullOrEmpty(yamlHeader.Status))
            {
                return content;
            }

            // Create a web friendly path for the content
            string contentWebPath = CreateContentWebPath(yamlHeader);

            // Build Content
            content = GetContent(content, yamlHeader, contentWebPath);

            // Return
            return content;
        }

        /// <summary>
        /// Create a path to a content page.
        /// </summary>
        /// <param name="contentYamlHeader">The object that represents contents metadata.</param>
        /// <returns>Returns a string that represents a web friendly path for the content.</returns>
        private static string CreateContentWebPath(ContentYamlHeader contentYamlHeader)
        {
            var pathSb = new StringBuilder(string.Join("-", contentYamlHeader.Title.Split(Path.GetInvalidFileNameChars())));
            pathSb.Replace(" ", "-");
            var path = pathSb.ToString().ToLowerInvariant();
            return path;
        }

        /// <summary>
        /// Appends the YamlHeader onto the Content object if Content is not null, Creates a content object if it is null
        /// </summary>
        /// <param name="content">The Content object.</param>
        /// <param name="contentYamlHeader">The ContentYamlHeader object.</param>
        /// <param name="contentWebPath">A string that represents a web friendly path for the content.</param>
        /// <returns>Returns a Content object.</returns>
        private static Content GetContent(Content content, ContentYamlHeader contentYamlHeader, string contentWebPath)
        {
            content = CheckContentIsNotNullAndCreate(content);
            content.Title = contentYamlHeader.Title;
            content.Author = contentYamlHeader.Author ?? string.Empty;
            content.Description = contentYamlHeader.Description ?? string.Empty;
            content.Type = contentYamlHeader.Type;
            content.Image = contentYamlHeader.Image ?? string.Empty;
            content.Thumbnail = contentYamlHeader.Thumbnail ?? string.Empty;
            content.Published = contentYamlHeader.Published;
            content.Categories = contentYamlHeader.Categories;
            content.Status = contentYamlHeader.Status;
            content.WebPath = contentWebPath;

            return content;
        }

        /// <summary>
        /// Checks if a Content object is null and Creates a Content object if it is null
        /// </summary>
        /// <param name="content">The Content object.</param>
        /// <returns>Returns a Content object.</returns>
        private static Content CheckContentIsNotNullAndCreate(Content content)
        {
            if (content == null) content = new Content();
            return content;
        }
    }
}
