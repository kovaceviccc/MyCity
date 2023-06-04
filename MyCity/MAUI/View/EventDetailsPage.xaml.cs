using MAUI.ViewModel;

namespace MAUI.View;

public partial class EventDetailsPage : ContentPage
{
	public EventDetailsPage(EventDetailsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		vm.Entry = entry;
	}

    protected override async void OnAppearing()
	{ 
		await ((EventDetailsPageViewModel)BindingContext).OnAppearingAsync();
    }
}