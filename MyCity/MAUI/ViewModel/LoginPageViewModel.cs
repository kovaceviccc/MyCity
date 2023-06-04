using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI.View;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;

namespace MAUI.ViewModel;

public partial class LoginPageViewModel : ObservableObject
{
    [ObservableProperty]
     private string userName;

    [ObservableProperty]
     private string password;

    [ObservableProperty]
    private string userNameLabel;

    [ObservableProperty]
    private string passwordLabel;

    [ObservableProperty]
    private bool isVisible = false; 

    private readonly IServiceProvider _serviceProvider;
    private readonly IApiHelper _apiHelper;

    public LoginPageViewModel(
        IServiceProvider serviceProvider,
        IApiHelper apiHelper)
    {
        _serviceProvider = serviceProvider;
        _apiHelper = apiHelper;
    }


    [RelayCommand]
    public async Task LoginAsync()
    {
        EmptyLabels();
        bool userInfo;
        bool authenticated;

        if (string.IsNullOrEmpty(UserName))
        {
            UserNameLabel = "Username cannot be empty!";
            return;
        }        

        if (string.IsNullOrEmpty(Password))
        {
            PasswordLabel = "You must enter a password field";
            return;
        }
        else if (Password?.Length < 7)
        {
            PasswordLabel = "Password must be at least 8 characters long";
            return;
        }

        try
        {
            authenticated = await _apiHelper.Authenticate(UserName, Password);
            userInfo = await _apiHelper.GetLoggedInUserInfo();
            
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            //log error
            return;
        }
        
        if (!(authenticated && userInfo)) 
        {
            await Shell.Current.DisplayAlert("Error", "Invalid credentials", "Ok");
            await UserSessionManager.LogofAsync();
            return;
        }

        IsVisible = false;
        await UserSessionManager.LoginAsync();
        EmptyInputFields();

        var mainPage = _serviceProvider.GetService<MainPage>();

        if (mainPage is null) return;

        Shell.Current.CurrentItem = mainPage;
    }

    [RelayCommand]
    public void NavigatedTo()
    {
        EmptyLabels();
    }


    public async Task SignUp_TappedAsync()
    {
        var registerPage = _serviceProvider.GetService<RegisterPage>();
        await Shell.Current.Navigation.PushModalAsync(registerPage);
    }

    private void EmptyLabels()
    {
        UserNameLabel = string.Empty;         
        PasswordLabel = string.Empty;
    }

    private void EmptyInputFields()
    {
        Password = string.Empty;
        UserName = string.Empty;
    }
}
