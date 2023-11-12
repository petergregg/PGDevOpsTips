using System.Text.Json.Serialization;

namespace PGDevOpsTips.Workflow.DTOs
{
    public class Author
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
