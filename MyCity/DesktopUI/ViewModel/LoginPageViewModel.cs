using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopUI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUI.ViewModel;

public partial class LoginPageViewModel : ObservableObject
{

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;







    private readonly RegisterPage _registerPage;

    [RelayCommand]
    public async void LoginAsync()
    {
        await Shell.Current.Navigation.PushAsync(_registerPage);
    }

    [RelayCommand]
    public void GoToRegisterPage()
    {

    }


    public LoginPageViewModel(RegisterPage registerPage)
    {
        _registerPage = registerPage;
    }
}
