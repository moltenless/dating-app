using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(4)]
    public string Password { get; set; } = null!;

    [Required]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;
    
    [Required] 
    public DateOnly DateOfBirth { get; set; }
}
