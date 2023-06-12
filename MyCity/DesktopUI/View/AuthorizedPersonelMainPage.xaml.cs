using DesktopUI.ViewModel;

namespace DesktopUI.View;

public partial class AuthorizedPersonelMainPage : ContentPage
{
    private bool isRunning;

	public AuthorizedPersonelMainPage(AuthorizedPersonelMainPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        vm.DiagramGrid = Diagram;
        vm.EmergencyFrame = EmergencyFrame;
        vm.RespondFrame = RespondFrame;

        vm.DisplayFrames.Add(EmergencyFrame);
        vm.DisplayFrames.Add(RespondFrame);
    }



    protected override async void OnAppearing()
    {
        await ((AuthorizedPersonelMainPageViewModel)BindingContext).OnAppearingAsync();
        base.OnAppearing();
    }

    private void Diagram_SizeChanged(object sender, EventArgs e)
    {
        if (isRunning) return;

        isRunning = true;

        ((AuthorizedPersonelMainPageViewModel)BindingContext).ResetUI();

        isRunning = false;
    }

    public AuthorizedPersonelMainPage()
    {
            
    }
}