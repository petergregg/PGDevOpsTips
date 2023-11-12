using System.Text.Json.Serialization;

namespace PGDevOpsTips.Workflow.DTOs
{
    public class Pusher
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
