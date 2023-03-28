using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    [Required]
    public override string? Email { get; set; }
    public int? Age { get; set; }
}
