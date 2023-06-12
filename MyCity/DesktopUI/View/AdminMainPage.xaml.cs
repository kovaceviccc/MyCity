using DesktopUI.ViewModel;

namespace DesktopUI.View;

public partial class AdminMainPage : ContentPage
{
    private bool IsRunning;
	public AdminMainPage(AdminMainPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		vm.DiagramGrid = Diagram;
        vm.RoleRequestFrame = RoleRequestsFrame;
        vm.EventsFrame = EventsFrame;



        vm.AllFrames.Add(RoleRequestsFrame);
        vm.AllFrames.Add(EventsFrame);
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
        if (IsRunning) return;

        IsRunning = true;

        ((AdminMainPageViewModel)BindingContext).ResetUI();

        IsRunning = false;
    }

}