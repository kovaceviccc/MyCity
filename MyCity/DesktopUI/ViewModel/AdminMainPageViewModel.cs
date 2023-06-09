using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopUI.Extensions;
using DesktopUI.Models;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models.Enums;
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

    private readonly IAdminEndpoint _adminEndpoint;

    [RelayCommand]
    public async Task OnAppearingAsync()
    {
        if(frames.Count == 0) 
        {
            foreach(var frame in DiagramGrid?.Children)
            {
                frames.Add((Frame)frame);
            }
        }

        var result = await _adminEndpoint.GetAllEvents(new TimeSpan(24,0,0));

        Events = result.Map();

        EventsCreated = Events.Count;
        Emergencies = Events.Where(e => e.EventType != EventTypeEnum.PublicEvent && e.EventType != EventTypeEnum.Party)
                            .Count();
        Responded = Events.Where(e => e.Responded == true).Count();
        ResetUI();
    }


    [RelayCommand]
    public void ResetUI()
    {
        if(frames.Count == 3)
        {
            frames[0].HeightRequest = DiagramGrid.Height * CaluclatePercentage(EventsCreated, EventsCreated);
            frames[1].HeightRequest = DiagramGrid.Height * CaluclatePercentage(Emergencies, EventsCreated);
            frames[2].HeightRequest = DiagramGrid.Height * CaluclatePercentage(Responded, EventsCreated);
        }
    }






    private double CaluclatePercentage(int value, int maxValue)
    {
        return (double)value / maxValue;
    }


    public AdminMainPageViewModel(IAdminEndpoint adminEndpoint)
    {
        _adminEndpoint = adminEndpoint;
    }
}
