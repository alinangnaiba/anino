using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Anino.Models;

public class ApiEndpoint
{
    [Required]
    public string Path { get; set; } = string.Empty;
    
    [Required]
    public string Method { get; set; } = string.Empty;
    
    [Range(100, 599)]
    public int StatusCode { get; set; } = 200;
    
    public JsonElement Response { get; set; }
}