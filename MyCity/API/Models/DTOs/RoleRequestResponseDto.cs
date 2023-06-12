namespace API.Models.DTOs;

public class RoleRequestResponseDto
{
    public required string RequestId { get; set; }
    public required string UserId { get; set; }
    public required string RoleId { get; set; }
    public required bool Approved { get; set; }
}
