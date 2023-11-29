using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

namespace project_k.Controllers;

[ApiController]
[Route("[controller]")]

public class FileController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileController(IWebHostEnvironment webHostEnvironment)
    {   
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost("upload")]
    public IActionResult UploadFile(IFormFile file)
    {
        if (file == null || file.Length <= 0)
        {
            return BadRequest(new { statusMessage = "Invalid file" });
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", "pdf" };
        var fileExtension = Path.GetExtension(file.FileName);

        if (!allowedExtensions.Contains(fileExtension.ToLower()))   
        {
            return BadRequest(new { statusMessage = "Invalid file format. Only jpg, jpeg, png, pdf formats are allowed" });
        }
        
        var fileName = DateTime.Now.Ticks.ToString() + fileExtension;

        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        using var fileStream = new FileStream(filePath, FileMode.Create);
        file.CopyTo(fileStream);
        return Ok(file);
        
        // return Ok(new { statusMessage = "File uploaded successfully" });
    }

    [HttpGet("download/{fileName}/{force}")]
    public IActionResult DownloadFile(string fileName, bool force)
    {
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(new { statusMessage = "File not found" });
        }
        var fileStream = System.IO.File.OpenRead(filePath);

        var contentType = force ? "application/octet-stream" : new FileExtensionContentTypeProvider().TryGetContentType(filePath, out var mimeType) ? mimeType : "application/octet-stream";

        var contentDispositionHeaderValue = new ContentDispositionHeaderValue(force ? "attachment" : "inline")
        {
            FileName = fileName,
        };

        Response.Headers["Content-Disposition"] = contentDispositionHeaderValue.ToString();

        return File(fileStream, contentType);
    }

    [HttpDelete("delete/{fileName}")]
    public IActionResult DeleteFile(string fileName)
    {
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
            return Ok(new { statusMessage = "File successfully deleted!" });
        }
        return NotFound(new { statusMessage = "File not found" });
    }
}