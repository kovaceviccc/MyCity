using DesktopUI.View;
using MAUI_Library.Helpers;

namespace DesktopUI
{
    public partial class App : Application
    {
        public App(AppShell shell,
                   IServiceProvider serviceProvider)
        {
            InitializeComponent();
            MainPage = shell;

            var loginPage = serviceProvider.GetService<LoginPage>();
            var registerPage = serviceProvider.GetService<RegisterPage>();

            if (loginPage is not null)
            {
                Shell.Current.Items.Add(loginPage);
            }

            if (registerPage is not null)
            {
                Shell.Current.Items.Add(registerPage);
            }

            //if (loginPage is not null)
            //{
            //    var shellContent = new ShellContent
            //    {
            //        ContentTemplate = new DataTemplate(() => loginPage)
            //    };

            //    var flyoutItem = new FlyoutItem
            //    {
            //        Title = "Login",
            //        Items = { shellContent }
            //    };

            //    Shell.Current.Items.Add(flyoutItem);

            //};
        }

        public override async void CloseWindow(Window window)
        {
            await UserSessionManager.LogofAsync();
            base.CloseWindow(window);
        }
    }
}