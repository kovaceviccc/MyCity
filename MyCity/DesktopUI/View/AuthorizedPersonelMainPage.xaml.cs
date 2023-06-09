using DesktopUI.ViewModel;

namespace DesktopUI.View;

public partial class AuthorizedPersonelMainPage : ContentPage
{
	public AuthorizedPersonelMainPage(AuthorizedPersonelMainPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    public AuthorizedPersonelMainPage()
    {
            
    }
}