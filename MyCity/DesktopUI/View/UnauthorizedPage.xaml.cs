using DesktopUI.ViewModel;

namespace DesktopUI.View;

public partial class UnauthorizedPage : ContentPage
{
	public UnauthorizedPage(UnauthorizedPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}


    protected override async void OnAppearing()
	{ 
		await ((UnauthorizedPageViewModel)BindingContext).OnAppearingAsync();
        base.OnAppearing();
    }
}