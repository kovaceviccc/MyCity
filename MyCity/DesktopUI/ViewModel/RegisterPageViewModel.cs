using CommunityToolkit.Mvvm.ComponentModel;

namespace DesktopUI.ViewModel;

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


}
