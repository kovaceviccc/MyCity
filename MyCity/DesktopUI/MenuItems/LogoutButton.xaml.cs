using MAUI_Library.Helpers;

namespace DesktopUI.MenuItems;

public partial class LogoutButton : MenuItem
{
	public LogoutButton()
	{
		InitializeComponent();
	}

    private async void ButtonClicked(object sender, EventArgs e)
    {
		await UserSessionManager.LogofAsync();
    }
}