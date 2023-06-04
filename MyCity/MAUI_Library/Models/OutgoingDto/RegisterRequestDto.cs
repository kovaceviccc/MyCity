using System.ComponentModel.DataAnnotations;
namespace MAUI_Library.Models.OutgoingDto;

public class RegisterRequestDto
{
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    [Required]
    public string DisplayName { get; set; } = default!;
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Email { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
}
