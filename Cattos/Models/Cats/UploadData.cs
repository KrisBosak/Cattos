namespace Cattos.Models.Cats
{
    public class UploadData
    {
        // There needs to be a way to have normal C# formatting... 
        public IFormFile File {  get; set; }
        public string breed_ids { get; set; } = string.Empty;
        public string sub_id { get; set; } = string.Empty;
    }
}
