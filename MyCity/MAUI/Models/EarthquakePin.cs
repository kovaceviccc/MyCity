using Bumptech.Glide.Request;
using MAUI_Library.Models.Incoming;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.Models;

public class EarthquakePin : CustomPin
{
    public Circle Circle{ get; set; }

    public EarthquakePin(EventModel eventModel,double radius = 30)
    {
        Id = eventModel.Id;
        Label = eventModel.Title;
        Address = eventModel.Description;
        Location = new Location(eventModel.Latitude, eventModel.Longitude);
        ImageSource = "MAUI/Resources/Images/earthquake2.svg";
        Circle = new Circle
        {
            StrokeWidth = 4,
            StrokeColor = Color.FromArgb("#E9BD17"),
            FillColor = Color.FromArgb("#66E9BD17"),
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
        map.Pins.Remove(this);
        map.MapElements.Remove(Circle);
    }
}
