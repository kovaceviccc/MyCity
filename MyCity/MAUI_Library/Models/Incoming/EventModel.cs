using MAUI_Library.Models.Enums;

namespace MAUI_Library.Models.Incoming;

public class EventModel
{
    public string Id { get; set; }
    public string PublisherId { get; set; }
    public string PublisherUserName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public EventTypeEnum EventType { get; set; }
    public List<string> Likes { get; set; }
}
