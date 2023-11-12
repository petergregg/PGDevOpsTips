using PGDevOpsTips.Domain.Models;
using PGDevOpsTips.Workflow.Interfaces;
using PGDevOpsTips.Workflow.Models;
using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PGDevOpsTips.Workflow.Services
{
    /// <inheritdoc/>
    public class YamlService : IYamlService
    {
        public Content YamlToContent(string plainFileContents, Content content)
        {
            // Check we have required props
            if (string.IsNullOrEmpty(content.Key))
            {
                throw new ArgumentException("Content.key property must already be set and is empty.", content.Key);
            }

            var yamlString = plainFileContents.Substring(0, plainFileContents.LastIndexOf("---\n")); // Chop off the markdown, leaving just the YAML header as YamlDotNet only deals with YAML documents. Assumes there is a space after the end of the YAML header

            var yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yamlHeader = yamlDeserializer.Deserialize<ContentYamlHeader>(yamlString);

            // Ensure we have a Title
            if (string.IsNullOrEmpty(yamlHeader.Title))
            {
                throw new ArgumentException("Title is a required field which is missing from the yaml header.", plainFileContents);
            }

            // Prepare categories to tags (url friendly)
            var tags = "";
            foreach (var cat in yamlHeader.Categories)
            {
                var tag = cat.Replace(' ', '-');
                tag = tag.ToLowerInvariant();
                tags += tag + ",";
            }
            tags = tags.TrimEnd(',');

            // Build Content
            if (content == null) content = new Content();
            content.Title = yamlHeader.Title;
            content.Author = yamlHeader.Author ?? string.Empty;
            content.Description = yamlHeader.Description ?? string.Empty;
            content.Image = yamlHeader.Image ?? string.Empty;
            content.Thumbnail = yamlHeader.Thumbnail ?? string.Empty;
            content.Published = yamlHeader.Published;
            content.Categories = tags;
            content.Type = yamlHeader.Type;
            content.Status = yamlHeader.Status;

            // Return
            return content;
        }
    }
}
