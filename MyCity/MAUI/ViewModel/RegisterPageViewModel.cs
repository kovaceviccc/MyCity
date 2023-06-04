using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models.OutgoingDto;

namespace MAUI.ViewModel;

public partial class RegisterPageViewModel : ObservableObject
{

    [ObservableProperty]
    private string firstName = "Petar";

    [ObservableProperty]
    private string lastName = "Kovacevic";

    [ObservableProperty]
    private string nickName = "perica";

    [ObservableProperty]
    private string email = "petar8617@gmail.com";

    [ObservableProperty]
    private string password = "perapera";

    [ObservableProperty]
    private string fistNameLabel;

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

    [RelayCommand]
    public async Task RegisterAsync()
    {
        EmptyLabels();

        

        if (string.IsNullOrEmpty(FirstName))
        {
            FistNameLabel = "Enter a valid name!";
            return;
        }

        if (string.IsNullOrEmpty(LastName))
        {
            LastNameLabel = "Enter a valid last name!";
            return;
        }

        if (string.IsNullOrEmpty(NickName))
        {
            NickNameLabel = "Enter a valid nickname";

            if (NickName?.Length > 10 || NickName?.Length < 4)
            {
                NickNameLabel = "Nickname must be between 4 and 12 charters";
                return;
            }
            return;
        }

        if (string.IsNullOrEmpty(Email))
        {
            EmailLabel = "Enter a valid email";
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
        var request = new RegisterRequestDto
        {
            FirstName = this.FirstName,
            LastName = this.LastName,
            DisplayName = this.NickName,
            Email = this.Email,
            Password = this.Password,
            DateOfBirth = this.DateOfBirth

        };

        bool userRegistered = await _authEndpoint.Register(request);

        if(userRegistered)
        {
            await Application.Current.MainPage.DisplayAlert("Registration successfull", "You can login now", "Ok");
            await Shell.Current.Navigation.PopModalAsync();
        }
    }



    private void EmptyLabels()
    {
        FistNameLabel = string.Empty;
        LastNameLabel = string.Empty;
        NickNameLabel = string.Empty;
        EmailLabel = string.Empty;
        PasswordLabel = string.Empty;
    }



    public RegisterPageViewModel(IAuthEndpoint authEndpoint)
    {
        _authEndpoint = authEndpoint;
    }
}
