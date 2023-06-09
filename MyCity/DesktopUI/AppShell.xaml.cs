using DesktopUI.MenuItems;
using DesktopUI.View;
using DesktopUI.ViewModel;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.Helpers;

namespace DesktopUI
{
    public partial class AppShell : Shell
    {

        public AppShell(AppShellViewModel vm,
                        IServiceProvider serviceProvider)
        {
            InitializeComponent();
            BindingContext = vm;

            var eventHub = serviceProvider.GetService<IEventHub>();
            var registerPage = serviceProvider.GetService<RegisterPage>();
            var loginPage = serviceProvider.GetService<LoginPage>();
            var adminMainPage = serviceProvider.GetService<AdminMainPage>();
            var authorizedPersonelMainPage = serviceProvider.GetService<AuthorizedPersonelMainPage>();
            var accountPage = serviceProvider.GetService<AccountPage>();
            var logoutButton = serviceProvider.GetService<LogoutButton>();
            var unauthorizedPage = serviceProvider.GetService<UnauthorizedPage>();


            if(eventHub is not null) UserSessionManager._eventHub = eventHub;

            if(registerPage is not null) UserSessionManager.RegisterPage = registerPage;

            if(loginPage is not null) UserSessionManager.LoginPage = loginPage;

            if(adminMainPage is not null) UserSessionManager.AdminMainPage = adminMainPage;

            if (accountPage is not null) UserSessionManager.AccountPage = accountPage;

            if(authorizedPersonelMainPage is not null) UserSessionManager.AuthorizedPersonelMainPage = authorizedPersonelMainPage;

            if(logoutButton is not null) UserSessionManager.LogOfButton = logoutButton;

            if (unauthorizedPage is not null) UserSessionManager.UnauthorizedPage = unauthorizedPage;
        }
    }
}