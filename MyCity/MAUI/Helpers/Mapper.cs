using Android.Mtp;
using MAUI.Models;
using MAUI.View;
using MAUI_Library.Models.Enums;
using MAUI_Library.Models.Incoming;
using Microsoft.Maui.Controls.Maps;
using System.Collections.ObjectModel;

namespace MAUI.Helpers;

public static class Mapper
{
    public static ObservableCollection<CustomPin> Map(this IEnumerable<EventModel> events)
    {
        var result = new ObservableCollection<CustomPin>();


        foreach (var eventModel in events) 
        {

            switch (eventModel.EventType)
            {
                case EventTypeEnum.Party:
                    result.Add(new PartyPin(eventModel));
                    break;

                case EventTypeEnum.Fire:
                    result.Add(new FirePin(eventModel));
                    break;

                case EventTypeEnum.Flood:
                    result.Add(new FloodPin(eventModel));
                    break;

                case EventTypeEnum.CarCrash:
                    result.Add(new CarCrashPin(eventModel));
                    break;

                case EventTypeEnum.Earthquake:
                    result.Add(new EarthquakePin(eventModel));
                    break;

                case EventTypeEnum.PublicEvent:
                    result.Add(new PublicEventPin(eventModel));
                    break;

                default:
                    break;
            }
        }
        return result;
    }

    public static CustomPin Map(this EventModel eventModel)
    {
        CustomPin result = null;

        switch (eventModel.EventType)
        {
            case EventTypeEnum.Party:
                result = new PartyPin(eventModel);
                break;

            case EventTypeEnum.Fire:
                result = new FirePin(eventModel);
                break;

            case EventTypeEnum.Flood:
                result = new FloodPin(eventModel);
                break;

            case EventTypeEnum.CarCrash:
                result = new CarCrashPin(eventModel);
                break;

            case EventTypeEnum.Earthquake:
                result =new EarthquakePin(eventModel);
                break;

            case EventTypeEnum.PublicEvent:
                result = new PublicEventPin(eventModel);
                break;

            default:
                break;

        }

        return result;
    }

    public static ObservableCollection<EventDisplayModel> Map(this IList<EventModel> eventModels)
    {
        ObservableCollection<EventDisplayModel> result = new();

        foreach(var model in eventModels)
        {
            string iconSource = string.Empty;

            switch (model.EventType)
            {
                case EventTypeEnum.Party:
                    iconSource = "MAUI/Resources/Images/party1.svg";
                    break;
                case EventTypeEnum.Fire:
                    iconSource = "MAUI/Resources/Images/fire_solid.svg";
                    break;
                case EventTypeEnum.Flood:
                    iconSource = "MAUI/Resources/Images/flood1.svg";
                    break;
                case EventTypeEnum.CarCrash:
                    iconSource = "MAUI/Resources/Images/car_crash.svg";
                    break;
                case EventTypeEnum.Earthquake:
                    iconSource = "MAUI/Resources/Images/earthquake2.svg";
                    break;
                case EventTypeEnum.PublicEvent:
                    iconSource = "MAUI/Resources/Images/publicevent.svg";
                    break;
                default:
                    break;
            }

            result.Add(new EventDisplayModel
            {
                Id = model.Id,
                PublisherId = model.PublisherId,
                EventTitle = model.Title,
                EventDescription = model.Description,
                DateCreated = model.DateCreated,
                Location = new Location
                {
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                },
                EventType = model.EventType,
                IconSource = iconSource,
                Likes = new(model.Likes)
            });
        }

        return result;
    }


    public static EventDisplayModel MapToDisplayModel(this EventModel eventModel)
    {
        string iconSource = string.Empty;

        switch (eventModel.EventType)
        {
            case EventTypeEnum.Party:
                iconSource = "MAUI/Resources/Images/party1.svg";
                break;
            case EventTypeEnum.Fire:
                iconSource = "MAUI/Resources/Images/fire_solid.svg";
                break;
            case EventTypeEnum.Flood:
                iconSource = "MAUI/Resources/Images/flood1.svg";
                break;
            case EventTypeEnum.CarCrash:
                iconSource = "MAUI/Resources/Images/car_crash.svg";
                break;
            case EventTypeEnum.Earthquake:
                iconSource = "MAUI/Resources/Images/earthquake2.svg";
                break;
            case EventTypeEnum.PublicEvent:
                iconSource = "MAUI/Resources/Images/publicevent.svg";
                break;
            default:
                break;
        }

        return new EventDisplayModel
        {
            Id = eventModel.Id,
            PublisherId = eventModel.PublisherId,
            PublisherUserName = eventModel.PublisherUserName,
            EventTitle = eventModel.Title,
            EventDescription = eventModel.Description,
            DateCreated = eventModel.DateCreated,
            Location = new Location
            {
                Latitude = eventModel.Latitude,
                Longitude = eventModel.Longitude
            },
            EventType = eventModel.EventType,
            IconSource = iconSource,
            Likes = new(eventModel.Likes)
        };
    }

    public static ObservableCollection<CommentDisplayModel> Map(this IEnumerable<CommentModel> comments)
    {
        ObservableCollection<CommentDisplayModel> result = new();

        foreach(var comment in comments)
        {
            result.Add(new CommentDisplayModel
            {
                EventId = comment.EventId,
                PublisherUserName = comment.PublisherUserName,
                Content = comment.Content,
                DateCreated = comment.DateCreated

            });
        }
        return result;
    }

    public static EventModel Map(this CustomPin pin)
    {
        return new EventModel
        {
            Id = pin.Id.ToString(),
            Title = pin.Address,
            DateCreated = pin.DateCreated,

        };
    }
}
