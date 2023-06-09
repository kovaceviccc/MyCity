using MAUI.ViewModel;
using MAUI.View;
using MAUI_Library.Helpers;
using MAUI.MenuItems;
using MAUI_Library.API.Hubs.Interfaces;

namespace MAUI;

public partial class AppShell : Shell
{
    private readonly LogofButton _logOfButton;

    public AppShell(LogofButton button,
                    AppShellViewModel vm,
                    LoginPage loginPage,
                    AccountPage accountPage,
                    IEventHub eventHub)
    {
        BindingContext = vm;
        InitializeComponent();
        _logOfButton = button;
        UserSessionManager.LogOfButton = _logOfButton;
        UserSessionManager._eventHub = eventHub;

        Routing.RegisterRoute(nameof(EventDetailsPage), typeof(EventDetailsPage));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void OnAppearingAsync(object sender, EventArgs e)
    {
        if (BindingContext is null) return;

        await ((AppShellViewModel)BindingContext).OnAppearingAsync();
    }
}