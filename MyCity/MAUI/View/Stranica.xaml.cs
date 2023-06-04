using MAUI.ViewModel;

namespace MAUI.View;

public partial class Stranica : ContentPage
{
	public Stranica(LoginPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm; 
	}

	public Stranica()
	{

	}
}