using System.Text.Json.Serialization;

namespace Cattos.Models.Cats
{
    public class Images
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = string.Empty;
        [JsonPropertyName("width")]
        public int Width { get; set; }
        [JsonPropertyName("height")]
        public int Height { get; set; }
        [JsonPropertyName("breeds")]
        public List<Breeds> Breeds { get; set; } = new();
    }
}
