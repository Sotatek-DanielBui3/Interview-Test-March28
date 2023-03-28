using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class AppUserRequest
{
    [Required]
    public string? FullName { get; set; }
    [Required]
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int? Age { get; set; }
}
