using Entities.Domain.Enums;

namespace API.Models.DTOs;

public class EventDto
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public EventTypeEnum EventType { get; set; }
}
