using MAUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.View;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsPageViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    public SettingsPage()
    {

    }

}