using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MAUI.Models;
using MAUI_Library.API.Interfaces;
using MAUI.Helpers;
using MAUI_Library.Models.Enums;
using MAUI_Library.Helpers;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.Models.Incoming;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotification;
using MAUI.View;

namespace MAUI.ViewModel;

public partial class EventDisplayViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<EventDisplayModel> eventsToDisplay = new();

    [ObservableProperty]
    private string filterOptionSelectedString = FilterOptionsEnum.MostRecent.ToString();

    private ObservableCollection<EventDisplayModel> allEvents = new();

    

    private readonly IEventEndpoint _eventEndpoint;
    private readonly IEventHub _eventHub;
    private List<FilterOptionsEnum> filterOptions = new();

    private int indexSelected = 0;

    private FilterOptionsEnum FilterOptionsSelected = FilterOptionsEnum.MostRecent;  

    [RelayCommand]
    public async Task GetEventsAsync()
    {
        var result = (await _eventEndpoint.GetAllEventsAsync(new TimeSpan(24, 0, 0))).ToList();

        allEvents = result.Map();

        await SortAsync();
    }

    [RelayCommand]
    public async Task MoveToDetailsPageAsync(EventDisplayModel eventModel)
    {
        if (eventModel is null) return;

        await Shell.Current.GoToAsync($"{nameof(EventDetailsPage)}", true, new Dictionary<string, object>
        {
            {"Event", eventModel.Id}
        });
    }

    [RelayCommand]
    public async Task FilterAsync()
    {
        indexSelected++;
        if(indexSelected >= filterOptions.Count)
        {
            indexSelected %= filterOptions.Count;
        }
        FilterOptionsSelected = filterOptions.ElementAt(indexSelected);
        FilterOptionSelectedString = FilterOptionsSelected.ToString();

        await SortAsync();
    }

    private async Task SortAsync()
    {
        switch (FilterOptionsSelected)
        {
            case FilterOptionsEnum.MyEvents:
                await SortMyEventsAsync();
                break;
            case FilterOptionsEnum.Closest:
                await SortByDistanceAsync();
                break;
            case FilterOptionsEnum.MostRecent:
                SortByDate();
                break;
            default:
                break;
        }
    }

    private async Task SortMyEventsAsync()
    {
        ObservableCollection<EventDisplayModel> result = new();
        string userId = await UserSessionManager.GetUserId();
        foreach(var eventModel in allEvents.Where(x=> x.PublisherId == userId))
        {
            result.Add(eventModel);
        }
        EventsToDisplay = result;
    }

    public async Task SortByDistanceAsync()
    {
        var userLocation = await UserSessionManager.GetUserLocationAsync();

        List<EventDisplayModel> result = new(allEvents.Count);

        result.AddRange(allEvents.OrderBy(p => CalculateDistance(userLocation.Latitude, userLocation.Longitude, p.Location.Latitude, p.Location.Longitude)).ToList());

        EventsToDisplay = new ObservableCollection<EventDisplayModel>(result);
    }


    private void SortByDate()
    {
        ObservableCollection<EventDisplayModel> result = new();

        foreach (var model in allEvents.OrderByDescending(x => x.DateCreated))
        {
            result.Add(model);
        }

        EventsToDisplay = result;
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Radius of the earth in km
        const double toRadians = Math.PI / 180;

        var dLat = (lat2 - lat1) * toRadians;
        var dLon = (lon2 - lon1) * toRadians;
        var lat1Rad = lat1 * toRadians;
        var lat2Rad = lat2 * toRadians;

        var a = Math.Pow(Math.Sin(dLat / 2), 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(dLon / 2), 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c; // Distance in km

        return d;
    }


    public EventDisplayViewModel(IEventEndpoint eventEndpoint,
                                 IEventHub eventHub)
    {
        _eventEndpoint = eventEndpoint;
        _eventHub = eventHub;

        _eventHub.Connection.On<EventModel>("EventRecived", EventRecivedAsync);

        var filterOptions = Enum.GetValues<FilterOptionsEnum>().ToList();

        if (filterOptions is null) return;
        this.filterOptions = filterOptions;
    }

    private async Task EventRecivedAsync(EventModel eventModel)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var result = eventModel.MapToDisplayModel();

            allEvents.Add(result);

            await SortAsync();
        });
    }
}
