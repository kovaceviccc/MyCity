using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Library.Models;

namespace DesktopUI.ViewModel;

public partial class AccountPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private LoggedInUserModel loggedInUserModel;
    
    public AccountPageViewModel(LoggedInUserModel loggedInUserModel)
    {
        LoggedInUserModel = loggedInUserModel;
        ResetUI();
    }

    public void NavigatedTo()
    {
        ResetUI();
    }

    [RelayCommand]
    public void ResetUI() 
    {
        Email = LoggedInUserModel.Email;
        FirstName = LoggedInUserModel.FirstName;
        LastName = LoggedInUserModel.LastName;
        UserName = LoggedInUserModel.UserName;
    }
}
