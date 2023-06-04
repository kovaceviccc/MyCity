using MAUI.ViewModel;
using MAUI_Library.Helpers;

namespace MAUI.MenuItems;

public partial class LogofButton : MenuItem
{
	public LogofButton(LogofButtonViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {

		if (BindingContext is null) return;

		await ((LogofButtonViewModel)BindingContext).ClickedAsync();
    }
}