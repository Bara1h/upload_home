namespace upload.Models
{
    public class FileMetadata
    {
        public int Id { get; set; }
        public string OriginalFileName { get; set; }
        public string Description { get; set; }
        public string UniqueFileName { get; set; }
    }

}
