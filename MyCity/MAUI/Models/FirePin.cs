using MAUI_Library.Models.Incoming;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;


namespace MAUI.Models;

public class FirePin : CustomPin
{
    public Circle Circle { get; set; }

	public FirePin(EventModel eventModel,double radius = 30)
	{
        Id = eventModel.Id;
        Label = eventModel.Title;
        Address = eventModel.Description;
        Location = new Location(eventModel.Latitude, eventModel.Longitude);
        ImageSource = "MAUI/Resources/Images/fire_solid.svg";
        Circle = new Circle
        {
            StrokeWidth = 4,
            StrokeColor = Color.FromArgb("#E25822"),
            FillColor = Color.FromArgb("#80E25822"),
            Center = base.Location,
            Radius = new Distance(radius),
        };
        PublisherId = eventModel.PublisherId;
        DateCreated = eventModel.DateCreated;
    }

    public override void Add(Map map)
    {
        map.MapElements.Add(Circle);
        map.Pins.Add(this);
    }

    public override void Remove(Map map)
    {
        map.Pins.Remove(this);
        map.MapElements.Remove(Circle);
    }
}
