using System.ComponentModel.DataAnnotations;

namespace TestWebApplication.Models;

public class PictureDto
{
    /// <summary>
    /// The picture file
    /// </summary>
    [Required]
    public IFormFile? FileToUpload { get; set; }

    /// <summary>
    /// Gets or Sets the picture properties
    /// </summary>
    [Required]
    public PictureItem Properties { get; set; } = new();
}