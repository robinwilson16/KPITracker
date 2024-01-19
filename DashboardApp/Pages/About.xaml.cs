namespace DashboardApp.Pages;

public partial class About : ContentPage
{
	public About()
	{
		InitializeComponent();
	}

    public async void ButtonRequestSupport_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://www.furthereducationpartnership.com");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    public void ButtonClose_Clicked(object sender, EventArgs e)
    {
        App.Current.CloseWindow(GetParentWindow());
    }
}