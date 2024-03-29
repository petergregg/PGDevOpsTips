﻿using PGDevOpsTips.Workflow.DTOs;
using System.Threading.Tasks;

namespace PGDevOpsTips.Workflow.Interfaces
{
    /// <summary>
    /// Service for interacting with Github.
    /// </summary>
    public interface IGithubService
    {
        /// <summary>
        /// Gets a content for a given file from the Github Content api
        /// </summary>
        /// <param name="fileApiUrl">The Github api url for the file to return.</param>
        /// <returns>Github content for the file.</returns>
        Task<GithubContent> GetGithubContent(string fileApiUrl);
    }
}
