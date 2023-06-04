using MAUI_Library.Models.Enums;

namespace MAUI_Library.Models.OutgoingDto;

public class EventDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double Latitude{ get; set; }
    public double Longitude { get; set; }
    public EventTypeEnum EventType { get; set; }
}
