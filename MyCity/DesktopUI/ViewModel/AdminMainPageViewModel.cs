using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopUI.Extensions;
using DesktopUI.Models;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models.Enums;
using MAUI_Library.Models.Incoming;
using MAUI_Library.Models.OutgoingDto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace DesktopUI.ViewModel;

public partial class AdminMainPageViewModel : ObservableObject
{

    [ObservableProperty]
    private Grid diagramGrid;

    private List<Frame> frames = new();

    [ObservableProperty]
    private int eventsCreated;

    [ObservableProperty]
    private int emergencies;

    [ObservableProperty]
    private int responded;

    [ObservableProperty]
    private int personel;

    [ObservableProperty]
    private ObservableCollection<BasicEventDisplayModel> events = new();

    [ObservableProperty]
    private ObservableCollection<BasicEventDisplayModel> eventsToDisplay = new();
    

    [ObservableProperty]
    private ObservableCollection<RoleRequestDisplayModel> roleRequests = new();

    [ObservableProperty]
    private Frame roleRequestFrame;

    [ObservableProperty]
    private Frame eventsFrame;

    [ObservableProperty]
    public List<Frame> allFrames = new();

    private readonly IAdminEndpoint _adminEndpoint;
    private readonly IMemoryCache _memoryCache;

    [RelayCommand]
    public async Task OnAppearingAsync()
    {
        var rolesTask = GetRoleRequests();
        
        if (frames.Count == 0 && DiagramGrid is not null)
        {
            foreach (var frame in DiagramGrid?.Children)
            {
                frames.Add((Frame)frame);
            }
        }

        Events = (await GetAllEvents()).Map();

        var roles = await rolesTask;
        RoleRequests = roles.Map();

        EventsCreated = Events.Count;
        Emergencies = Events.Where(e => e.EventType != EventTypeEnum.PublicEvent && e.EventType != EventTypeEnum.Party)
                            .Count();
        Responded = Events.Where(e => e.Responded == true).Count();
        ResetUI();
    }

    [RelayCommand]
    public void ShowUserRequests()
    {
        //if (RoleRequestFrame.IsEnabled && RoleRequestFrame.IsVisible) return;

        foreach (var frame in AllFrames)
        {
            frame.IsVisible = false;
            frame.IsEnabled = false;
        }
        RoleRequestFrame.IsEnabled = true;
        RoleRequestFrame.IsVisible = true;
    }

    [RelayCommand]
    public void ShowRespondedEvents()
    {
        EventsToDisplay = new(Events.Where(x => x.Responded == true));
        ShowEventsFrame();
    }

    [RelayCommand]
    public void ShowEmergencyEvents()
    {
        EventsToDisplay = new(Events.Where(x => x.EventType != EventTypeEnum.PublicEvent && x.EventType != EventTypeEnum.Party));
        ShowEventsFrame();
    }

    [RelayCommand]
    public void ShowAllEvents()
    {
        EventsToDisplay = Events;
        ShowEventsFrame();
    }


    [RelayCommand]
    public void ResetUI()
    {
        if (frames.Count == 3)
        {
            frames[0].HeightRequest = DiagramGrid.Height * CaluclatePercentage(EventsCreated, EventsCreated);
            frames[1].HeightRequest = DiagramGrid.Height * CaluclatePercentage(Emergencies, EventsCreated);
            frames[2].HeightRequest = DiagramGrid.Height * CaluclatePercentage(Responded, EventsCreated);
        }
    }

    [RelayCommand]
    public async Task AcceptRoleRequest(RoleRequestDisplayModel request)
    {
        await RespondToRoleRequest(true, request);
    }

    [RelayCommand]
    public async Task DenyRoleRequest(RoleRequestDisplayModel request)
    {
        await RespondToRoleRequest(false, request);
    }

    private async Task<IEnumerable<BasicEventModel>> GetAllEvents()
    {
        if (_memoryCache.TryGetValue("AllEvents", out IEnumerable<BasicEventModel> cachedEvents))
        {
            return cachedEvents;
        }

        try
        {
            var result = await _adminEndpoint.GetAllEvents(new TimeSpan(24, 0, 0));

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


    private async Task<IEnumerable<RoleRequestModel>> GetRoleRequests()
    {

        if (_memoryCache.TryGetValue("RoleRequests", out IEnumerable<RoleRequestModel> cachedRoles))
        {
            return cachedRoles;
        }

        var rolesTask = _adminEndpoint.GetAllRoleRequests();
        (bool success, IEnumerable<RoleRequestModel> roles) = await rolesTask;

        if (success)
        {
            // Cache the result
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));
            _memoryCache.Set("RoleRequests", roles, cacheOptions);

            return roles;
        }

        return Enumerable.Empty<RoleRequestModel>();
    }
    private void ShowEventsFrame()
    {
        foreach (var frame in AllFrames)
        {
            frame.IsVisible = false;
            frame.IsEnabled = false;
        }

        EventsFrame.IsEnabled = true;
        EventsFrame.IsVisible = true;
    }

    private async Task RespondToRoleRequest(bool approved, RoleRequestDisplayModel request)
    {

        var result = await _adminEndpoint.RespondToRoleRequest(new RoleRequestRespondDto
        {
            RequestId = request.Id,
            UserId = request.UserId,
            RoleId = request.RoleName,
            Approved = approved
        });

        if (result)
        {
            await Shell.Current.DisplayAlert("Success", $"Role {request.RoleName} succefully added to user{request.UserUserName}", "Ok");
            _memoryCache.Remove("RoleRequests");
            RoleRequests = (await GetRoleRequests()).Map();
        }
    }

    private double CaluclatePercentage(int value, int maxValue)
    {
        return (double)value / maxValue;
    }


    public AdminMainPageViewModel(IAdminEndpoint adminEndpoint,
                                  IMemoryCache memoryCache)
    {
        _adminEndpoint = adminEndpoint;
        _memoryCache = memoryCache;
    }
}
