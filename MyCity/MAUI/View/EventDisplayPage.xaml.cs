using Android.Views.Animations;
using MAUI.ViewModel;

namespace MAUI.View;

public partial class EventDisplayPage : ContentPage
{
	public EventDisplayPage(EventDisplayViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

		await ((EventDisplayViewModel)BindingContext).GetEventsAsync();
    }
}