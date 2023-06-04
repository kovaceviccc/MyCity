using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MAUI_Library.Models.Incoming;
namespace MAUI.Models;

public class PublicEventPin : CustomPin
{
    public Circle Circle { get; set; }

    public PublicEventPin(EventModel eventModel, double radius = 30)
    {
        Id = eventModel.Id;
        Label = eventModel.Title;
        Address = eventModel.Description;
        Location = new Location(eventModel.Latitude, eventModel.Longitude);
        ImageSource = "MAUI/Resources/Images/publicevent.svg";
        Circle = new Circle
        {
            StrokeWidth = 4,
            StrokeColor = Color.FromArgb("#AE9756"),
            FillColor = Color.FromArgb("#80AE9756"),
            Center = Location,
            Radius = new Distance(radius),
        };
        PublisherId = eventModel.PublisherId;
        DateCreated = eventModel.DateCreated;
    }

    public override void Add(Microsoft.Maui.Controls.Maps.Map map)
    {
        map.MapElements.Add(Circle);
        map.Pins.Add(this);
    }

    public override void Remove(Microsoft.Maui.Controls.Maps.Map map)
    {
        map.MapElements.Remove(Circle);
        map.Pins.Remove(this);
    }
}
