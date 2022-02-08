using System.Text.Json;

namespace PGDevOpsTips.Web
{
    public static class DefaultJsonSerializerOptions
    {
        public static JsonSerializerOptions Options => new() { PropertyNameCaseInsensitive = true };
    }
}
