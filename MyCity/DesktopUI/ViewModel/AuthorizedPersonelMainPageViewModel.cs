using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopUI.Extensions;
using DesktopUI.Models;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models.Incoming;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using MAUI_Library.Helpers;

namespace DesktopUI.ViewModel;

public partial class AuthorizedPersonelMainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private Grid diagramGrid;

    [ObservableProperty]
    private int eventsResponded;

    [ObservableProperty]
    private int respondedByCurrentUser;

    private List<Frame> frames = new();

    [ObservableProperty]
    private ObservableCollection<BasicEventDisplayModel> emergencies = new();

    [ObservableProperty]
    private ObservableCollection<BasicEventDisplayModel> eventsToDisplay = new();

    [ObservableProperty]
    private ObservableCollection<BasicEventDisplayModel> emergenciesResponded = new();

    [ObservableProperty]
    private Frame emergencyFrame;

    [ObservableProperty]
    private Frame respondFrame;

    [ObservableProperty]
    private List<Frame> displayFrames = new();

    private readonly IAdminEndpoint _adminEndpoint;
    private readonly IMemoryCache _memoryCache;
    private readonly IEventHub _eventHub;

    public async Task OnAppearingAsync()
    {

        if (frames.Count == 0 && DiagramGrid is not null)
        {

            var subGrid = (Grid)DiagramGrid?.Children[0];

            DiagramGrid = subGrid;

            foreach(var frame in DiagramGrid?.Children)
            {
                frames.Add((Frame)frame);
            }

            //foreach (var frame in DiagramGrid?.Children)
            //{
            //    frames.Add((Frame)frame);
            //}
        }

        await GetRequiredData();
    }

    private async Task GetRequiredData()
    {
        var result = (await GetAllEvents()).Map();

        Emergencies = new(result.OrderBy(x=>x.Responded));

        EmergenciesResponded = new(result.Where(x=> x.Responded == true));

        EventsResponded = EmergenciesResponded.Count;



        ResetUI();
    }

    [RelayCommand]
    public async Task RespondToEmergencyEvent(BasicEventDisplayModel e)
    {

        var procced = (await Shell.Current.DisplayAlert("Attention!", "Are you sure you want to confirm that help for selected event will be sent", "Ok", "Cancel"));

        if (!procced) return;

        try
        {
            var res = await _eventHub.RespondToEventAsync(e.Id);

            if (!res) await Shell.Current.DisplayAlert("Error", "Error occured while processing request!\nPlease try again", "Ok");

            _memoryCache.Remove("AllEvents");
            await GetRequiredData();
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Error", "Error occured while processing request! \nPlease try again", "Ok");
        }
    }


    public void ResetUI()
    {
        if (frames.Count == 3)
        {
            frames[0].HeightRequest = DiagramGrid.Height * CaluclatePercentage(Emergencies.Count, Emergencies.Count);
            frames[1].HeightRequest = DiagramGrid.Height * CaluclatePercentage(EventsResponded, Emergencies.Count);
            frames[2].HeightRequest = DiagramGrid.Height * CaluclatePercentage(RespondedByCurrentUser, Emergencies.Count);
        }
    }

    [RelayCommand]
    public void ShowEmergencyFrame()
    {
        foreach(var frame in DisplayFrames)
        {
            frame.IsVisible = false;
            frame.IsEnabled = false;
        }

        EmergencyFrame.IsVisible = true;
        EmergencyFrame.IsEnabled = true;
    }

    [RelayCommand]
    public void ShowRespondFrame()
    {
        foreach (var frame in DisplayFrames)
        {
            frame.IsVisible = false;
            frame.IsEnabled = false;
        }

        RespondFrame.IsVisible = true;
        RespondFrame.IsEnabled = true;
    }

    private async Task<IEnumerable<BasicEventModel>> GetAllEvents()
    {
        if (_memoryCache.TryGetValue("AllEvents", out IEnumerable<BasicEventModel> cachedEvents))
        {
            return cachedEvents;
        }

        try
        {
            var result = await _adminEndpoint.GetAllEmergencies(new TimeSpan(24, 0, 0));

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));
            _memoryCache.Set("AllEvents", result, cacheOptions);

            return result;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            return Enumerable.Empty<BasicEventModel>();
        }
    }

    private double CaluclatePercentage(int value, int maxValue)
    {
        return (double)value / maxValue;
    }
    
    private async Task EventRespondedAsync(string eventId, string responderId)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            foreach(var emergency in Emergencies)
            {
                if(emergency.Id == eventId)
                {

                    if(responderId == await UserSessionManager.GetUserId())
                    {
                        await Shell.Current.DisplayAlert("Success!", "You confirmed that the help is on the way!\nThank you for your service", "Ok");
                    }
                    _memoryCache.Remove("AllEvents");
                    await GetRequiredData();
                    return;
                }
            }
        });
    }

    public AuthorizedPersonelMainPageViewModel(IAdminEndpoint adminEndpoint,
                                               IMemoryCache memoryCache,
                                               IEventHub eventHub)
    {
        _adminEndpoint = adminEndpoint;
        _memoryCache = memoryCache;
        _eventHub = eventHub;

        _eventHub.Connection.On<string, string>("EventResponded", EventRespondedAsync);
    }
}
