namespace API.Models.DTOs;

public class ChangePasswordRequestDto
{
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
