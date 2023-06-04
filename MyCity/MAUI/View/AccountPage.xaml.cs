using MAUI.ViewModel;

namespace MAUI.View;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }

	public AccountPage()
	{

	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
		((AccountPageViewModel)BindingContext).NavigatedTo();
    }

    private void OldPasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = (AccountPageViewModel)BindingContext;
        viewModel.UpdateButtonEnabled();
    }

    private void NewPasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = (AccountPageViewModel)BindingContext;
        viewModel.UpdateButtonEnabled();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        MyStackLayout.IsVisible=!MyStackLayout.IsVisible;
    }
}