namespace WebApi.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;

    public Member Member { get; set; } = null!;
}

