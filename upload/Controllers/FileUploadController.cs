using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using upload.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using upload.Models;

public class FileUploadController : Controller
{
    private readonly FileDbContext _context;
    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    public FileUploadController(FileDbContext context)
    {
        _context = context;
    }

    // GET: FileUpload
    public IActionResult Index()
    {
        var viewModel = new FileUploadIndexViewModel
        {
            FileUploadViewModel = new FileUploadViewModel(),
            Files = _context.FileMetadatas.ToList()
        };

        return View(viewModel);  // Pass the combined model to the view
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(FileUploadViewModel model)
    {
        // Check if the file is null or has no content
        if (model.File == null || model.File.Length == 0)
        {
            ModelState.AddModelError("File", "No file selected.");
        }
        else
        {
            // Validate file type
            var allowedExtensions = new[] { ".pdf", ".png", ".jpg", ".docx" };
            var fileExtension = Path.GetExtension(model.File.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", "Invalid file type. Allowed types are .pdf, .png, .jpg, .docx.");
            }

            // Validate file size (5MB max)
            if (model.File.Length > 5 * 1024 * 1024) // 5MB
            {
                ModelState.AddModelError("File", "File size exceeds the 5MB limit.");
            }
        }

        // Validate description
        if (string.IsNullOrEmpty(model.Description))
        {
            ModelState.AddModelError("Description", "Description is required.");
        }

        // If validation fails, return to the view with errors
        if (!ModelState.IsValid)
        {
            var viewModel = new FileUploadIndexViewModel
            {
                FileUploadViewModel = model,
                Files = _context.FileMetadatas.ToList()
            };
            return View(nameof(Index), viewModel);
        }

        // Generate unique file name
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName).ToLower();
        var filePath = Path.Combine(_uploadPath, uniqueFileName);

        // Ensure the upload directory exists
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }

        // Save the file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await model.File.CopyToAsync(stream);
        }

        // Save file metadata in the database
        var fileMetadata = new FileMetadata
        {
            OriginalFileName = model.File.FileName,
            Description = model.Description,
            UniqueFileName = uniqueFileName
        };

        _context.FileMetadatas.Add(fileMetadata);
        await _context.SaveChangesAsync();

        // Provide success feedback
        TempData["SuccessMessage"] = "File uploaded successfully.";
        return RedirectToAction(nameof(Index));
    }


    // File download action
    public IActionResult Download(int id)
    {
        var fileMetadata = _context.FileMetadatas.Find(id);
        if (fileMetadata == null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(_uploadPath, fileMetadata.UniqueFileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream", fileMetadata.OriginalFileName);
    }
}
