using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models.OutgoingDto;

namespace MAUI.ViewModel;

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
    private string oldPassword;

    [ObservableProperty]
    private string newPassword;

    private readonly IAuthEndpoint _authEndpoint;

    private readonly LoggedInUserModel _loggedInUserModel;
	public AccountPageViewModel(LoggedInUserModel loggedInUserModel, IAuthEndpoint authEndpoint)
	{
        _loggedInUserModel = loggedInUserModel;
        ResetUI();
        _authEndpoint = authEndpoint;
    }
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
    

    private bool _isButtonEnabled;
    public bool IsButtonEnabled
    {
        get { return _isButtonEnabled; }
        set
        {
            _isButtonEnabled = value;
            OnPropertyChanged(nameof(IsButtonEnabled));
        }
    }
    public void UpdateButtonEnabled()
    {
        IsButtonEnabled = !(string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(NewPassword));
    }   

    public void NavigatedTo()
	{
		ResetUI();
    }

	private void ResetUI()
	{
        UserName = _loggedInUserModel.UserName;
		Email = _loggedInUserModel.Email;
        FirstName = _loggedInUserModel.FirstName;
        LastName = _loggedInUserModel.LastName;
    }
}
