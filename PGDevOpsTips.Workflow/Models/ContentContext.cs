using PGDevOpsTips.Domain.Models;
using PGDevOpsTips.Workflow.DTOs;
using System;

namespace PGDevOpsTips.Workflow.Models
{
    /// <summary>
    /// A model for collating data related to Content as it passes through the pipeline.
    /// </summary>
    public class ContentContext
    {
        /// <summary>
        /// The full Uri to get information about a given file from the Github content Api
        /// </summary>
        public Uri GithubContentApiUri { get; set; }

        /// <summary>
        /// Result of calling the Github Content api for the given content. Contains many usefull properties such as path, download uri, contents and name
        /// </summary>
        public GithubContent GithubContent { get; set; }

        /// <summary>
        /// The plain (decoded) content of the file.
        /// </summary>
        public string PlainContents { get; set; }

        /// <summary>
        /// The plain (decoded) content of the file converted to html.
        /// </summary>
        public string PlainHtmlContents { get; set; }

        /// <summary>
        /// The uri for the html blob in storage
        /// </summary>
        public string HtmlBlobStorageUri { get; set; }

        /// <summary>
        /// Name of the HTML blob in storage, for exmaple test.html
        /// </summary>
        public string HtmlBlobFileName { get; set; }

        /// <summary>
        /// Content entitiy which gets stored and used by other systems.
        /// </summary>
        public Content Content { get; set; }
    }
}
