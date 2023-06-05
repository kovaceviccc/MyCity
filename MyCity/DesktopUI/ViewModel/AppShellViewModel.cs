using CommunityToolkit.Mvvm.ComponentModel;
using DesktopUI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUI.ViewModel;

public partial class AppShellViewModel : ObservableObject
{
    private readonly LoginPage _loginPage;
    private readonly RegisterPage _registerPage;




    public async Task OnAppearingAsync()
    {
        //await Task.Delay(500);
    }



    public AppShellViewModel(LoginPage loginPage,
                             RegisterPage registerPage)
    {
        _loginPage = loginPage;
        _registerPage = registerPage;
    }
}
