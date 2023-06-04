using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models.Enums;
using MAUI_Library.Models.OutgoingDto;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace MAUI.ViewModel;

public partial class EventPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string eventTitle;

    [ObservableProperty]
    private string eventTitleLabel;

    [ObservableProperty]
    private string eventDescription;

    [ObservableProperty]
    private string eventDecriptionLabel;

    [ObservableProperty]
    private Picker picker = new();

    [ObservableProperty]
    private EventTypeEnum selectedType;

    [ObservableProperty]
    private IList<EventTypeEnum> eventTypes = new List<EventTypeEnum>();

    [ObservableProperty]
    private IList<double> radiusValues = new List<double>();

    [ObservableProperty]
    private double selectedRadius;

    [ObservableProperty]
    private bool isRadiusPickerVisible = false;

    [ObservableProperty]
    public Map map;

    public Location Location { get; set; }
    private readonly IEventHub _eventHub;

    [RelayCommand]
    public async Task AddEventAsync()
    {
        bool valid = ValidateInputFields();
        if (!valid) return;

        Location ??= await UserSessionManager.GetUserLocationAsync();

        var eventDto = new EventDto
        {
            Title = EventTitle,
            Description = EventDescription,
            Latitude = Location.Latitude,
            Longitude = Location.Longitude,
            EventType = SelectedType
        };

        var result = await _eventHub.AddEvent(eventDto);

        if (result)
        {
            await Shell.Current.Navigation.PopModalAsync();
            return;
        }

        await Shell.Current.DisplayAlert("Error!", "Event not added succesfully", "OK");
        return;
    }

    private bool ValidateInputFields()
    {
        if(string.IsNullOrEmpty(EventTitle)) 
        {
            EventTitleLabel = "Please add event title";
            return false;
        }
        else if(EventTitle?.Length < 4)
        {
            EventTitleLabel = "Event title must be ad least 4 charachers";
            return false;
        }

        if(string.IsNullOrEmpty(EventDescription)) 
        {
            EventDecriptionLabel = "Please add event description";
            return false;
        }
        else if(EventDescription?.Length < 4)
        {
            EventDecriptionLabel = "Event description must be at least 4 charachters";
            return false;
        }
        return true;
    }

    public void SelectedEventChanged()
    {
        if (SelectedType is EventTypeEnum.Party || SelectedType is EventTypeEnum.PublicEvent)
        {
            IsRadiusPickerVisible = false;
            return;
        }
        IsRadiusPickerVisible = true;
    }

    public EventPageViewModel(IEventHub eventHub)
    {
        _eventHub = eventHub;

        var events = (Enum.GetValues<EventTypeEnum>()).ToList();

        events.Remove(EventTypeEnum.AllEvents);

        for (double i = 0; i < 1000; i += 10)
        {
            RadiusValues.Add(i);

        }

        if (events is null) return;

        EventTypes = events;
    }
}
