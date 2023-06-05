using DesktopUI.View;
using DesktopUI.ViewModel;
using MAUI_Library.API.Hubs;
using MAUI_Library.API.Hubs.Interfaces;
using MAUI_Library.Helpers;

namespace DesktopUI
{
    public partial class AppShell : Shell
    {
        private readonly LoginPage _loginPage;
        private readonly RegisterPage _registerPage;

        public AppShell(AppShellViewModel vm,
            LoginPage loginPage, RegisterPage registerPage, IEventHub eventHub)
        {
            InitializeComponent();
            BindingContext = vm;
            _loginPage = loginPage;
            _registerPage = registerPage;

            UserSessionManager._eventHub = eventHub;
        }

        protected override async void OnAppearing()
        {
            await ((AppShellViewModel)BindingContext).OnAppearingAsync();
            
            var result = await UserSessionManager.CheckCredentialsAsync();
            if (!result) 
            {
                Current.Items.Add(_loginPage);
                Current.Items.Add(_registerPage);
            }
        }
    }
}