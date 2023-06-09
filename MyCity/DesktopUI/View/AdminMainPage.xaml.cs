using DesktopUI.ViewModel;

namespace DesktopUI.View;

public partial class AdminMainPage : ContentPage
{
	public AdminMainPage(AdminMainPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		vm.DiagramGrid = Diagram;

	} 

    protected override async void OnAppearing()
    {
        await ((AdminMainPageViewModel)BindingContext).OnAppearingAsync();
    }

    public AdminMainPage()
    {
			
    }

    private void Diagram_SizeChanged(object sender, EventArgs e)
    {
        ((AdminMainPageViewModel)BindingContext).ResetUI();
    }
}