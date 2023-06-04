using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs;

public class UserRegistrationRequestDto
{
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    [Required]
    public string DisplayName{ get; set; } = default!;
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Email { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;

}
