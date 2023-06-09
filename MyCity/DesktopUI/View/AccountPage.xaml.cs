using DesktopUI.ViewModel;

namespace DesktopUI.View;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ((AccountPageViewModel)BindingContext).NavigatedTo();
        base.OnNavigatedTo(args);
    }

    public AccountPage()
    {
            
    }
}