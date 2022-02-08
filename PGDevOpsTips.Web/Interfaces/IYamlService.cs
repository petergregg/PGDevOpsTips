using PGDevOpsTips.Web.Models;

namespace PGDevOpsTips.Web.Interfaces
{
    /// <summary>
    /// Service for working with YAML.
    /// </summary>
    public interface IYamlService
    {
        /// <summary>
        /// Converts YAML to an Content object.
        /// </summary>
        /// <param name="plainFileContents">The plain file contents which contains the YAML.</param>
        /// <param name="content">The Content to append the YAML data onto.</param>
        /// <returns>Content.</returns>
        public Content YamlToContent(string plainFileContents, Content content);
    }
}
