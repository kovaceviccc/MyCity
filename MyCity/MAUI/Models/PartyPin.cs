using MAUI_Library.Models.Incoming;

namespace MAUI.Models;

public class PartyPin : CustomPin
{
    public PartyPin(EventModel eventModel)
    {
        Id = eventModel.Id;
        Label = eventModel.Title;
        Address = eventModel.Description;
        Location = new Location(eventModel.Latitude, eventModel.Longitude);
        ImageSource = "MAUI/Resources/Images/party1.svg";
        PublisherId = eventModel.PublisherId;
        DateCreated = eventModel.DateCreated;
    }

    public override void Add(Microsoft.Maui.Controls.Maps.Map map)
    {
        map.Pins.Add(this);
    }

    public override void Remove(Microsoft.Maui.Controls.Maps.Map map)
    {
        map.Pins.Remove(this);
    }
}
