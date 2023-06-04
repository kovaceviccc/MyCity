namespace Entities.DbSet;

public class Comment : BaseEntity
{
    public User User { get; set; } = default!;
    public Event Event { get; set; } = default!;
    public string Content { get; set; } = default!;
}
