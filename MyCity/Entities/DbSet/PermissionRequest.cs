using Microsoft.AspNetCore.Identity;

namespace Entities.DbSet;

public class PermissionRequest : BaseEntity
{
    public required User User { get; set; }
    public required IdentityRole Role { get; set; }
    public bool Responded { get; set; } = false;
    public bool Approved { get; set; } = false;
}
