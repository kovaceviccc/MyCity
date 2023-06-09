using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Library.API.Interfaces;
using MAUI_Library.Models.Incoming;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUI.ViewModel;

public partial class UnauthorizedPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<RoleModel> roles;

    [ObservableProperty]
    private RoleModel selectedRole;

    private readonly IAdminEndpoint _adminEndpoint;

    [RelayCommand]
    public async Task SubmmitRequestAsync()
    {
        if(SelectedRole is null)
        {
            await Shell.Current.DisplayAlert("Error", "Please pick the role you want", "Ok");
            return;
        }

        (bool res, string error) = await _adminEndpoint.SubmitRequestAsync(SelectedRole.Id);

        if (!res)
        {
            await Shell.Current.DisplayAlert("Error", error, "OK");
            return;
        }

        await Shell.Current.DisplayAlert("Success", "You have succesfully submited a request!", "Ok");
    }

    public async Task OnAppearingAsync()
    {

        var result = await _adminEndpoint.GetAllRequiredRoles();

        Roles = new(result);
    }




    public UnauthorizedPageViewModel(IAdminEndpoint adminEndpoint)
    {
        _adminEndpoint = adminEndpoint;
    }
}
