using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs;

public class TokenRequest
{
    [Required]
    public string Token { get; set; } = default!;
    [Required]
    public string RefreshToken { get; set; } = default!;
}
