using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PGDevOpsTips.Workflow.DTOs
{
    public class HeadCommit
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("tree_id")]
        public string TreeId { get; set; }

        [JsonPropertyName("distinct")]
        public bool Distinct { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("committer")]
        public Committer Committer { get; set; }

        [JsonPropertyName("added")]
        public List<string> Added { get; set; }

        [JsonPropertyName("removed")]
        public List<string> Removed { get; set; }

        [JsonPropertyName("modified")]
        public List<string> Modified { get; set; }
    }
}
