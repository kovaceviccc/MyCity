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

            //services 

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginPageViewModel>();

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterPageViewModel>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AppShellViewModel>();

            //library services
            builder.Services.AddSingleton<LoggedInUserModel>();
            builder.Services.AddSingleton<IApiHelper, ApiHelper>();

            //endpoints
            builder.Services.AddSingleton<IEventEndpoint, EventEndpoint>();
            builder.Services.AddSingleton<IAuthEndpoint, AuthEndpoint>();

            //SignalR servisi(hubovi)
            builder.Services.AddSingleton<IEventHub, EventHub>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}