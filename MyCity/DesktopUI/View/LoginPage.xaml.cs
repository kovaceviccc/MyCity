using DesktopUI.ViewModel;

namespace DesktopUI.View;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}