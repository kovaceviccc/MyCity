using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models.OutgoingDto;

namespace MAUI.ViewModel;

public partial class SettingsPageViewModel : ObservableObject
{
    /*[ObservableProperty]
    private string oldPassword;

    [ObservableProperty]
    private string newPassword;

    private readonly IAuthEndpoint _authEndpoint;

[RelayCommand]
public async Task ChangePasswordAsync()
{
    var request = new ChangePasswordRequestDto
    {
        OldPassword = OldPassword,
        NewPassword = NewPassword
    };
    var result = await _authEndpoint.ChangePassword(request);

    if (!result) return;

    var res = !(await Shell.Current.DisplayAlert("", "Please login to add events", "Cancel", "Ok"));
}



public SettingsPageViewModel(IAuthEndpoint authEndpoint)
{
    _authEndpoint = authEndpoint;
}*/
}