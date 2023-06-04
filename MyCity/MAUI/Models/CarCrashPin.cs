using MAUI_Library.Models.Incoming;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MAUI.Models;

public class CarCrashPin : CustomPin
{
	public Circle Circle{ get; set; }

	public CarCrashPin(EventModel eventModel,double radius = 30)
	{
        Id = eventModel.Id;
        Label = eventModel.Title;
        Address = eventModel.Description;
        Location = new Location(eventModel.Latitude, eventModel.Longitude);
        ImageSource = "MAUI/Resources/Images/car_crash.svg";
        Circle = new Circle
        {
            StrokeWidth = 4,
            StrokeColor = Color.FromArgb("#d0342c"),
            FillColor = Color.FromArgb("#80F07579"),
            Center = Location,
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
