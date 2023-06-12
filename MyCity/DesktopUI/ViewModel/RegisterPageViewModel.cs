using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopUI.View;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models.OutgoingDto;

namespace DesktopUI.ViewModel;

public partial class RegisterPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string firstName = "Danijela";

    [ObservableProperty]
    private string lastName = "Kovacevic";

    [ObservableProperty]
    private string email = "danijelakovacevic675@gmail.com";

    [ObservableProperty]
    private string password = "Danijela675_";

    [ObservableProperty]
    private string firstNameLabel;

    [ObservableProperty]
    private string lastNameLabel;

    [ObservableProperty]
    private string nickNameLabel;

    [ObservableProperty]
    private string emailLabel;

    [ObservableProperty]
    private string passwordLabel;

    [ObservableProperty]
    private DateTime dateOfBirth;

    private readonly IAuthEndpoint _authEndpoint;
    private readonly LoginPage _loginPage;

    [RelayCommand]
    public async Task Register()
    {
        ClearLabels();
        var registerRequest = new RegisterRequestDto
        {
            FirstName = FirstName,
            LastName = LastName,
            DisplayName = FirstName,
            DateOfBirth = DateOfBirth,
            Email = Email,
            Password = Password,
        };

        bool isValid = registerRequest.ValidateRequest(out var errors);

        if(isValid)
        {
            try
            {
                var result = await _authEndpoint.Register(registerRequest);

                if (result)
                {
                    await Shell.Current.DisplayAlert("Succesfully registered an account", "Please login to continue", "Ok");
                    Shell.Current.CurrentItem = _loginPage;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed", $"Error: {ex.Message}", "Ok");
                return;
            }
        }
        else
        {
            UpdateLabels(errors);
        }

        
    }

    private void ClearLabels()
    {
        FirstNameLabel = string.Empty;
        LastNameLabel = string.Empty;
        EmailLabel = string.Empty;
        PasswordLabel = string.Empty;
        NickNameLabel = string.Empty;
    }

    private void UpdateLabels(Dictionary<string,string> errors)
    {
        if (errors.ContainsKey("FirstName")) FirstNameLabel = errors["FirstName"];
        if (errors.ContainsKey("LastName")) LastNameLabel = errors["LastName"];
        if (errors.ContainsKey("Email")) EmailLabel = errors["Email"];
        if (errors.ContainsKey("Password")) PasswordLabel = errors["Password"];
        if (errors.ContainsKey("DisplayName")) NickNameLabel = errors["DisplayName"];
    }


    ////TODO: add fluen validation for the fields
    //private bool ValidateInputFields()
    //{
    //    if(FirstName.Length < 2)
    //    {
    //        FirstNameLabel = "Please enter a valid name";
    //    }

    //    if(LastName.Length < 2)
    //    {
    //        LastNameLabel = "Please enter a valid last name";
    //    }

    //    if(Email.Length < 5)
    //}


    public RegisterPageViewModel(IAuthEndpoint authEndpoint,
                                 LoginPage loginPage)
    {
        _authEndpoint = authEndpoint;
        _loginPage = loginPage;
    }


}
