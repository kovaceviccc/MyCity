using MAUI.ViewModel;

namespace MAUI.View;

public partial class LoginPage : ContentPage
{

    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();  

        BindingContext = vm;
    }

    public LoginPage()
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        ((LoginPageViewModel)BindingContext).NavigatedTo();
    }

    private async void SignUp_Tapped(object sender, TappedEventArgs e)
    {
        await ((LoginPageViewModel)BindingContext).SignUp_TappedAsync();
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {
        base.OnNavigatedFrom(e);
        ((LoginPageViewModel)BindingContext).NavigatedTo();
    }
}