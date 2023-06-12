using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopUI.View;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUI.ViewModel;

public partial class LoginPageViewModel : ObservableObject
{

    [ObservableProperty]
    private string email = "danijelakovacevic675@gmail.com"; //string.Empty;

    [ObservableProperty]
    private string password = "Danijela675_"; //string.Empty;

    [ObservableProperty]
    private string emailLabel = string.Empty;

    [ObservableProperty]
    private string passwordLabel = string.Empty;

    private readonly IApiHelper _apiHelper;
    private readonly IServiceProvider _serviceProvider;

    [RelayCommand]
    public async Task LoginAsync()
    {
        ResetLabels();
        bool userInfo;
        bool authenticated;
        if(Email?.Length < 4)
        {
            EmailLabel = "Please enter a valid email";
            return;
        }

        if(Password?.Length < 7)
        {
            PasswordLabel = "Password must be atleast 8 charachters long";
            return;
        }

        try
        {
            authenticated = await _apiHelper.Authenticate(Email, Password);
            userInfo = await _apiHelper.GetLoggedInUserInfo();
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            await Shell.Current.DisplayAlert("Error", error, "Ok");
            return;
        }

        if(!(authenticated && userInfo))
        {
            await Shell.Current.DisplayAlert("Error", "Invalid credentials", "Ok");
            await UserSessionManager.LogofAsync();
            return;
        }

        ResetUI();
        await UserSessionManager.LoginAsync();
    }

    [RelayCommand]
    public void GoToRegisterPage()
    {
        var registerPage = _serviceProvider.GetService<RegisterPage>();

        if (registerPage is null) return;

        Shell.Current.CurrentItem = registerPage;
    }

    private void ResetUI()
    {
        Email = string.Empty;
        Password = string.Empty;
    }

    private void ResetLabels()
    {

        EmailLabel = string.Empty;
        PasswordLabel = string.Empty;
    }

    public LoginPageViewModel(IServiceProvider serviceProvider,
                              IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
        _serviceProvider = serviceProvider;
    }
}
