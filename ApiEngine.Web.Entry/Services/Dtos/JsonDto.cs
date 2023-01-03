using System.ComponentModel.DataAnnotations;

namespace ApiEngine.Web.Entry.Services.Dtos;

public class JsonDto
{
    [Required]
    public string json { get; set; }
}