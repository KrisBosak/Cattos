using System.Text.Json.Serialization;

namespace Cattos.Models.Cats
{
    public class Breeds
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("life_span")]
        public string LifeSpan { get; set; } = string.Empty;
        [JsonPropertyName("breed_group")]
        public string BreedGroup { get; set; } = string.Empty;
    }
}
