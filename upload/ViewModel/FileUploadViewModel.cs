using System.ComponentModel.DataAnnotations;
using upload.Models;

public class FileUploadViewModel
{
    public IFormFile File { get; set; }
    public string Description { get; set; }

}

public class FileUploadIndexViewModel
{
    public FileUploadViewModel FileUploadViewModel { get; set; }
    public IEnumerable<FileMetadata> Files { get; set; }

}