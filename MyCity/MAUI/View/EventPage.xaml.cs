using MAUI.ViewModel;

namespace MAUI.View;

public partial class EventPage : ContentPage
{
	public EventPage()
	{

	}

	public EventPage(EventPageViewModel vm)
	{ 
		BindingContext = vm;
		InitializeComponent();
	}

	//private async void Button_Clicked(object sender, EventArgs e)
	//{
	//	await ((EventPageViewModel)BindingContext).AddEventAsync(Location);
	//}

	private void SelectedEventChanged(object sender, EventArgs e)
	{ 
		((EventPageViewModel)BindingContext).SelectedEventChanged();
    }
}