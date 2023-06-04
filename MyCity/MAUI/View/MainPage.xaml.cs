using MAUI.ViewModel;
using MAUI_Library.Helpers;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace MAUI.View;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
        vm.Mapa = mappy;
        vm.Content = Content;
        BindingContext = vm;

	}

    public MainPage()
    {

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var location = await UserSessionManager.GetUserLocationAsync();

        if (location == null) return;

        var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(200));
         
        mappy.MoveToRegion(mapSpan);
        await ((MainPageViewModel)BindingContext).OnAppearingAsync();
    }


    private async void MapClickedAsync(object sender, MapClickedEventArgs e)
    {
        await ((MainPageViewModel)BindingContext).MapClickedAsync(e.Location);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}