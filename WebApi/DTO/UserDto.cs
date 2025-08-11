using System;

namespace WebApi.DTO;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string Token { get; set; } = null!;
}
