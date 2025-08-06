using Microsoft.AspNetCore.Identity;

namespace WebApi.Entities;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpire { get; set; }

    public Member Member { get; set; } = null!;
}

