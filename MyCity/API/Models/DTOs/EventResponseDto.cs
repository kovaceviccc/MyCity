using Entities.Domain.Enums;
using System.Text;

namespace API.Models.DTOs;

public class EventResponseDto
{
    public string Id { get; set; } = default!;
    public string PublisherId { get; set; } = default!;
    public string PublisherUserName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime DateCreated { get; set; }
    public double Latitude { get; set; } = default!;
    public double Longitude { get; set; } = default!;
    public EventTypeEnum EventType { get; set; }
    public List<string> Likes { get; set; } = new();

}
