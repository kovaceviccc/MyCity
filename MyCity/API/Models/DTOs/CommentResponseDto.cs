namespace API.Models.DTOs;

public class CommentResponseDto
{
    public string EventId { get; set; } = default!;
    public string PublisherUserName { get; set; } = default!;
    public DateTime DateCreated { get; set; }
    public string Content { get; set; } = default!;
}
