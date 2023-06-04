using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI.MenuItems;
using MAUI.View;
using MAUI_Library.API.Hubs;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.Helpers;
using MAUI_Library.Models;

namespace MAUI.ViewModel;

public partial class AppShellViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isEnabled = true;

    private readonly LoggedInUserModel _user;
    private readonly IServiceProvider _services;
    private readonly IEventHub _eventHub;

    public AppShellViewModel(LoggedInUserModel user,
                             IServiceProvider services,
                             IEventHub eventHub)
    {
        _user = user;
        _services = services;
        _eventHub = eventHub;
    }

    //[RelayCommand]
    //public void Logof()
    //{
    //    UserSessionManager.Logof();
    //    var eventMap = _services.GetService<MainPage>();
    //    Shell.Current.CurrentItem = eventMap;
    //}

    [RelayCommand]
    public async Task OnAppearingAsync()
    {
        var res = await UserSessionManager.CheckCredentialsAsync();

        if (!res) return;

        await _user.GetBasicUserModelAsync();
    }
}
