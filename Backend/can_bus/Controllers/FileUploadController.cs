using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class FileUploadController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is not selected or empty.");
        }

        // Specify the directory where the file will be saved
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Generate a unique file name to prevent overwriting
        string uniqueFileName = Path.Combine(uploadsFolder, Path.GetRandomFileName());

        // Save the file to disk
        using (var fileStream = new FileStream(uniqueFileName, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // Return the file path or any other response as needed
        return Ok(new { FilePath = uniqueFileName });
    }
}
