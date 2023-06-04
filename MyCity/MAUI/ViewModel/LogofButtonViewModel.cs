using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI.Helpers;
using MAUI.View;
using MAUI_Library.Helpers;

namespace MAUI.ViewModel;

public partial class LogofButtonViewModel : ObservableObject
{
    private readonly MainPage _mainPage;
    private readonly AccountPage _accountPage;

    [RelayCommand]
    public async Task ClickedAsync()
    {
        if (Shell.Current.CurrentItem == (ShellItem)_accountPage)
        {
            Shell.Current.CurrentItem = (ShellItem)_mainPage;
        }

        await UserSessionManager.LogofAsync();
    }


    public LogofButtonViewModel(MainPage mainPage,
                                AccountPage accountPage)
    {
        _mainPage = mainPage;
        _accountPage = accountPage;
    }
}
