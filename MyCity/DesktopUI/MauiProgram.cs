using DesktopUI.MenuItems;
using DesktopUI.View;
using DesktopUI.ViewModel;
using MAUI_Library.API;
using MAUI_Library.API.Endpoints;
using MAUI_Library.API.Hubs;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models;
using Microsoft.Extensions.Logging;

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