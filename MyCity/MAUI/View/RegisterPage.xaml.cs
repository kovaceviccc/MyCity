using MAUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.View;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    private async void RegisterButton_Clicked(object sender, EventArgs e)
    {
        await ((RegisterPageViewModel)BindingContext).RegisterAsync();
    }
}
