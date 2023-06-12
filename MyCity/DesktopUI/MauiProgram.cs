using DesktopUI.MenuItems;
using DesktopUI.View;
using DesktopUI.ViewModel;
using MAUI_Library.API;
using MAUI_Library.API.Endpoints;
using MAUI_Library.API.Hubs;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace DesktopUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            
            events.AddWindows(windowsLifecycleBuilder =>
            {
                windowsLifecycleBuilder.OnWindowCreated(window =>
                {
                    window.ExtendsContentIntoTitleBar = true;
                    var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                    switch (appWindow.Presenter)
                    {
                        case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                            overlappedPresenter.SetBorderAndTitleBar(true, true);
                            overlappedPresenter.IsResizable = false;
                            overlappedPresenter.Maximize();
                            break;
                    }
                });
            });
        });
#endif

            //cache
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

            //application pages

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginPageViewModel>();

            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<RegisterPageViewModel>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddSingleton<AdminMainPage>();
            builder.Services.AddSingleton<AdminMainPageViewModel>();

            builder.Services.AddSingleton<AuthorizedPersonelMainPage>();
            builder.Services.AddSingleton<AuthorizedPersonelMainPageViewModel>();

            builder.Services.AddSingleton<AccountPageViewModel>();
            builder.Services.AddSingleton<AccountPage>();

            builder.Services.AddSingleton<UnauthorizedPageViewModel>();
            builder.Services.AddSingleton<UnauthorizedPage>(); 

            //menu items
            builder.Services.AddSingleton<LogoutButton>();

            //library services
            builder.Services.AddSingleton<LoggedInUserModel>();
            builder.Services.AddSingleton<IApiHelper, ApiHelper>();

            //endpoints
            builder.Services.AddSingleton<IEventEndpoint, EventEndpoint>();
            builder.Services.AddSingleton<IAuthEndpoint, AuthEndpoint>();
            builder.Services.AddSingleton<IAdminEndpoint, AdminEndpoint>();

            //SignalR servisi(hubovi)
            builder.Services.AddSingleton<IEventHub, EventHub>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}