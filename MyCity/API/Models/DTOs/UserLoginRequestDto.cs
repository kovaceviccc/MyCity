using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs;

public class UserLoginRequestDto
{
    [Required]
    public string Email { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
}
