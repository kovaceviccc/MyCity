using Entities.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities.DbSet;

public class User : IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = default!;

    public List<RefreshToken> RefreshTokens { get; set; } = new();

    [JsonIgnore]
    public List<Event> Events { get; set; } = new();

    public List<Like> Likes { get; set; } = new();


    [Required]
    [MaxLength(10)]
    public string DisplayName { get; set; } = default!;
    [Required]
    public DateTime DateOfBirth { get; set; } = default!;
    public bool IsBlocked { get; set; } = false;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? DeletedAtUtc { get; set; }

}
