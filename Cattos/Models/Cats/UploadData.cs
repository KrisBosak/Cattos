using System.Text.Json.Serialization;

namespace Cattos.Models.Cats
{
    public class UploadData
    {
        [JsonPropertyName("file")]
        public string File {  get; set; } = string.Empty;
        [JsonPropertyName("breed_ids")]
        public string BreedIds { get; set; } = string.Empty;
        [JsonPropertyName("sub_id")]
        public string Username { get; set; } = string.Empty;
    }
}
