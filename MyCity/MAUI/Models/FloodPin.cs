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

public class FloodPin : CustomPin
{
    public Circle Circle { get; set; }

	public FloodPin(EventModel eventModel)
	{
        Id = eventModel.Id;
        Label = eventModel.Title;
        Address = eventModel.Description;
        Location = new Location(eventModel.Latitude, eventModel.Longitude);
        ImageSource = "MAUI/Resources/Images/flood1.svg";
        Circle = new Circle
        {
            StrokeWidth = 4,
            StrokeColor = Color.FromArgb("#0677DC"),
            FillColor = Color.FromArgb("#804891D3"),
            Center = base.Location,
            Radius = new Distance(30),
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
