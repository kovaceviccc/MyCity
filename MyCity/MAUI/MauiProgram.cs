 using MAUI.ViewModel;
using MAUI.View;
using Microsoft.Extensions.Logging;
using LocalizationResourceManager.Maui;
using System.Globalization;
using Microsoft.VisualStudio.Settings;
using System.Runtime.CompilerServices;
using MAUI_Library.API.Interfaces;
using MAUI_Library.API;
using MAUI_Library.Models;
using Microsoft.VisualStudio.RpcContracts.Build;
using MAUI.MenuItems;
using MAUI_Library.API.Endpoints;
using MAUI.Helpers;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.API.Hubs;
using Plugin.LocalNotification;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Android.Telephony.Data;

namespace MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()               
                .UseMauiMaps()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //custom handlers
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
            });

            //var assembly = Assembly.GetExecutingAssembly();
            //using var stream = assembly.GetManifestResourceStream("MAUI.appsettings.json");

            //var config = new ConfigurationBuilder()
            //         .AddJsonStream(stream)
            //         .Build();

            //builder.Configuration.AddConfiguration(config);


            //MAUI app services
            builder.Services.AddSingleton<RegisterPageViewModel>();
            builder.Services.AddSingleton<RegisterPage>();

            builder.Services.AddSingleton<MainPageViewModel>();    
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<AccountPageViewModel>();
            builder.Services.AddSingleton<AccountPage>();

            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<LoginPage>();

            builder.Services.AddSingleton<SettingsPageViewModel>();
            builder.Services.AddSingleton<SettingsPage>();

            builder.Services.AddSingleton<EventPageViewModel>();
            builder.Services.AddSingleton<EventPage>();

            builder.Services.AddSingleton<EventDisplayPage>();
            builder.Services.AddSingleton<EventDisplayViewModel>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddTransient<EventDetailsPage>();
            builder.Services.AddTransient<EventDetailsPageViewModel>();


            //haklovanje
            builder.Services.AddSingleton<LogofButton>();
            builder.Services.AddSingleton<LogofButtonViewModel>();
            builder.Services.AddSingleton<Microsoft.Maui.Controls.Maps.Map>();
            

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