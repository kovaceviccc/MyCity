using System.ComponentModel.DataAnnotations;

namespace Entities.DbSet;

public class RefreshToken : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string JwtId { get; set; } = default!;
    public bool IsUsed { get; set; } = false;
    public bool IsRevoked { get; set; } = false;
    public DateTime AddedDate { get; set; }
    public DateTime ExpireDate { get; set; } = default!;
}
