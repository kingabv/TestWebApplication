using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace TestWebApplication.Models;

public class PictureItem
{
    [IgnoreDataMember]
    [SwaggerSchema(ReadOnly = true)]
    public byte[]? File { get; set; }

    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "Please enter valid integer Number")]
    public long Id { get; set; }
    [Required]
    [StringLength(30, ErrorMessage = "Max 30 characters")]
    public string? Name { get; set; }

    [DefaultValue(true)]
    [Required]
    public bool IsComplete { get; set; }
}