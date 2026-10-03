using System.ComponentModel.DataAnnotations;

namespace Cognia.Shared.Models;

public record UserResponse(
    string Id, string Email, string DisplayName, IReadOnlyList<string> Roles, DateTime CreatedAt);

public class CreateUserRequest
{
    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>Optional role to assign (e.g. "Admin"). The role must already exist.</summary>
    public string? Role { get; set; }
}

public class UpdateUserRequest
{
    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;
}
