using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI.Helpers;
using MAUI.Models;
using MAUI.View;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models.Incoming;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using Plugin.LocalNotification;
using Map = Microsoft.Maui.Controls.Maps.Map;
using MAUI_Library.Models.Enums;
using Microsoft.Maui.Controls.Maps;
using Android.Mtp;
using System.Net.NetworkInformation;

namespace MAUI.ViewModel;

public partial class MainPageViewModel : ObservableObject
{

    

    [ObservableProperty]
    private Map mapa;

    [ObservableProperty]
    private ObservableCollection<CustomPin> pinsToDisplay = new();

    private ObservableCollection<CustomPin> AllPins = new();

    [ObservableProperty]
    private string eventTypeSelectedString = EventTypeEnum.AllEvents.ToString();

    private EventTypeEnum FilterOptionSelected = EventTypeEnum.AllEvents;

    private readonly List<EventTypeEnum> EventTypeFilterOptions = new();

    private int EventTypeIndexSelected = 0;

    private readonly EventPage _eventPage;
    private readonly IEventEndpoint _eventEndpoint;
    private readonly IEventHub _eventHub;
    private readonly EventPageViewModel _eventPageViewModel;
    private readonly IServiceProvider _serviceProvider;

    public Microsoft.Maui.Controls.View Content { get; internal set; }

    private bool PinClicked = false;

    [RelayCommand]
    public async Task AddEventAsync(Location loc)
    {
        if (await SecureStorage.GetAsync("token") is null || await SecureStorage.GetAsync("refresh_token") is null)
        {
            var res = !(await Shell.Current.DisplayAlert("", "Please login to add events", "Cancel", "Ok"));
            if(res) 
            {
                var loginPage = _serviceProvider.GetService<LoginPage>();

                if(loginPage is null) 
                {
                    //handle error better
                    return;
                } 
                await UserSessionManager.LogofAsync();
                Shell.Current.CurrentItem = loginPage;
                return;
            }
            return;
        }
        _eventPageViewModel.Location = loc;

        await Shell.Current.Navigation.PushModalAsync(_eventPage);
    }

    [RelayCommand]
    public async Task OnAppearingAsync()
    {
        if(Mapa.Pins.Count == 0)
        {
            AllPins = (await _eventEndpoint.GetAllEventsAsync(new TimeSpan(24, 0, 0))).Map();

            foreach (var pin in AllPins)
            {
                pin.InfoWindowClicked += PinInfoWindowClickedAsync;
                pin.MarkerClicked += PinMarkerClickedAsync;
            }

            Sort();
        }
    }

    [RelayCommand]
    public void FilterByType()
    {
        EventTypeIndexSelected++;
        if (EventTypeIndexSelected >= EventTypeFilterOptions.Count)
        {
            EventTypeIndexSelected %= EventTypeFilterOptions.Count;
        }
        FilterOptionSelected = EventTypeFilterOptions.ElementAt(EventTypeIndexSelected);
        EventTypeSelectedString = FilterOptionSelected.ToString();

        Sort();
    }

    [RelayCommand]
    public void MapDragged()
    {
        Content.Focus();
    }

    private void Sort()
    {
        switch (FilterOptionSelected)
        {
            case EventTypeEnum.Party:
                SortPartyEvents();
                break;
            case EventTypeEnum.Fire:
                SortFireEvents();
                break;
            case EventTypeEnum.PublicEvent:
                SortPublicEvents();
                break;
            case EventTypeEnum.Flood:
                SortFloodEvents();
                break;
            case EventTypeEnum.CarCrash:
                SortCarCrashEvents();
                break;
            case EventTypeEnum.Earthquake:
                SortEarthquakeEvents();
                break;
            case EventTypeEnum.AllEvents:
                PinsToDisplay = AllPins;
                AddAllPins();
                break;
            default:
                break;
        }
    }

    private void AddAllPins()
    {
        foreach (var p in PinsToDisplay)
        {
            if (!Mapa.Pins.Contains(p))
            {
                p.Add(Mapa);
            }
        }
    }

    private void SortEarthquakeEvents()
    {
        PinsToDisplay = new();

        foreach(var pin in AllPins)
        {

            if(pin.GetType() == typeof(EarthquakePin))
            {
                if (!Mapa.Pins.Contains(pin))
                {
                    PinsToDisplay.Add(pin);
                    pin.Add(Mapa);
                }
            }
            else
            {
                if(Mapa.Pins.Contains(pin))
                {
                    pin.Remove(Mapa);
                }
            }
        }
    }

    private void SortCarCrashEvents()
    {
        PinsToDisplay = new();

        foreach (var pin in AllPins)
        {

            if (pin.GetType() == typeof(CarCrashPin))
            {
                if (!Mapa.Pins.Contains(pin))
                {
                    PinsToDisplay.Add(pin);
                    pin.Add(Mapa);
                }
            }
            else
            {
                if (Mapa.Pins.Contains(pin))
                {
                    pin.Remove(Mapa);
                }
            }
        }
    }

    private void SortFloodEvents()
    {
        PinsToDisplay = new();

        foreach (var pin in AllPins)
        {

            if (pin.GetType() == typeof(FloodPin))
            {
                if (!Mapa.Pins.Contains(pin))
                {
                    PinsToDisplay.Add(pin);
                    pin.Add(Mapa);
                }
            }
            else
            {
                if (Mapa.Pins.Contains(pin))
                {
                    pin.Remove(Mapa);
                }
            }
        }
    }

    private void SortPublicEvents()
    {
        PinsToDisplay = new();

        foreach (var pin in AllPins)
        {

            if (pin.GetType() == typeof(PublicEventPin))
            {
                if (!Mapa.Pins.Contains(pin))
                {
                    PinsToDisplay.Add(pin);
                    pin.Add(Mapa);
                }
            }
            else
            {
                if (Mapa.Pins.Contains(pin))
                {
                    pin.Remove(Mapa);
                }
            }
        }
    }

    private void SortFireEvents()
    {
        PinsToDisplay = new();

        foreach (var pin in AllPins)
        {

            if (pin.GetType() == typeof(FirePin))
            {
                if (!Mapa.Pins.Contains(pin))
                {
                    PinsToDisplay.Add(pin);
                    pin.Add(Mapa);
                }
            }
            else
            {
                if (Mapa.Pins.Contains(pin))
                {
                    pin.Remove(Mapa);
                }
            }
        }
    }

    private void SortPartyEvents()
    {
        PinsToDisplay = new();

        foreach (var pin in AllPins)
        {

            if (pin.GetType() == typeof(PartyPin))
            {
                if (!Mapa.Pins.Contains(pin))
                {
                    PinsToDisplay.Add(pin);
                    pin.Add(Mapa);
                }
            }
            else
            {
                if (Mapa.Pins.Contains(pin))
                {
                    pin.Remove(Mapa);
                }
            }
        }
    }

    public async Task SortByDistanceAsync()
    {
        var userLocation = await UserSessionManager.GetUserLocationAsync();

        List<CustomPin> result = new(AllPins.Count);

        result.AddRange(AllPins.OrderBy(p => CalculateDistance(userLocation.Latitude, userLocation.Longitude, p.Location.Latitude, p.Location.Longitude)).ToList());

        PinsToDisplay = new ObservableCollection<CustomPin>(result);
    }


    private void SortByDate()
    {
        ObservableCollection<CustomPin> result = new();

        foreach (var model in AllPins.OrderByDescending(x => x.DateCreated))
        {
            result.Add(model);
        }

        PinsToDisplay = result;
    }


    public async Task MapClickedAsync(Location loc)
    {

        if (PinClicked)
        {
            PinClicked = false;
            return;
        }

        var placemarks = await Geocoding.GetPlacemarksAsync(loc);
        var street = placemarks?
                    .Where(x => x.Thoroughfare is not null &&
                               x.SubThoroughfare is not null &&
                               int.TryParse(x.SubThoroughfare, out int res) &&
                               x.Location is not null)
                    .OrderByDescending(p => CalculateDistance(loc.Latitude, loc.Longitude, p.Location.Latitude, p.Location.Longitude))
                    .FirstOrDefault();

        await AddEventAsync(loc);
    }

    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Radius of the earth in km
        var dLat = (lat2 - lat1) * Math.PI / 180;  // deg2rad below
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
            ;
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c; // Distance in km
        return d;
    }

    private async Task EventRecivedAsync(EventModel eventModel)
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {

            var eventRecived = eventModel.Map();
            AllPins.Add(eventRecived);

            if(eventRecived.GetType().ToString() == FilterOptionSelected.ToString()+"Pin" || FilterOptionSelected == EventTypeEnum.AllEvents)
            {
                PinsToDisplay.Add(eventRecived);
                eventRecived.Add(Mapa);
            }
            eventRecived.InfoWindowClicked += PinInfoWindowClickedAsync;
            eventRecived.MarkerClicked += PinMarkerClickedAsync;

        });
    }

    private async void PinInfoWindowClickedAsync(object sender, PinClickedEventArgs e)
    {
        try
        {
            var pin = (CustomPin)sender;
            


            await Shell.Current.GoToAsync($"{nameof(EventDetailsPage)}", true, new Dictionary<string, object>
            {
                {"Event", pin.Id}
            });
        }
        catch (Exception)
        {
            return;
        }
    }

    private async void PinMarkerClickedAsync(object sender, PinClickedEventArgs e)
    {
        PinClicked = true;
    }

    public MainPageViewModel(EventPage eventPage,
                          IEventEndpoint eventEndpoint,
                          IEventHub eventHub,
                          EventPageViewModel eventPageViewModel,
                          IServiceProvider serviceProvider)
    {
        _eventPage = eventPage;
        _eventEndpoint = eventEndpoint;
        _eventHub = eventHub;
        _eventPageViewModel = eventPageViewModel;
        _serviceProvider = serviceProvider;

        EventTypeFilterOptions = Enum.GetValues<EventTypeEnum>().ToList();


        //signalR part
        _eventHub.Connection.On<EventModel>("EventRecived", EventRecivedAsync);
    }
}
