using MAUI_Library.Models.Enums;
using System.Collections.ObjectModel;

namespace MAUI.Models;


public class EventDisplayModel
{
    public string Id { get; set; }
    public string PublisherId { get; set; }
    public string PublisherUserName { get; set; }
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public DateTime DateCreated { get; set; }
    public EventTypeEnum EventType { get; set; }
    public Location Location { get; set; }
    public string IconSource { get; set; }
    public ObservableCollection<string> Likes { get; set; }
}
