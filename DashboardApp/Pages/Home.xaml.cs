using DashboardApp.Services;
using DashboardApp.Shared;
using System.Diagnostics;

namespace DashboardApp.Pages;

public partial class Home : ContentPage
{
	public Home()
	{
		InitializeComponent();
        LoadDashboard("Home");
    }

    public void LoadDashboard(string dashboard)
    {
        string? dashboardSource = App.Configuration.GetSection("DashboardPages")[dashboard].ToString();
        if (dashboardSource != null)
        {
            DashboardWebpage.Source = dashboardSource;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (GlobalVariables.IsInitialised == false)
        {
            while (GlobalVariables.IsDbContextValid == true && GlobalVariables.IsDbContextBusy == true)
            {
                Trace.WriteLine("Database is busy");
                await Task.Delay(25);
            }
            if (GlobalVariables.IsDbContextValid == true)
            {
                GlobalVariables.IsDbContextBusy = true;
                var auditAdded = await AddAuditRecord();
                GlobalVariables.IsDbContextBusy = false;

                GlobalVariables.IsInitialised = true;
            }
        }
    }

    public void OnNavigating(object sender, WebNavigatingEventArgs e)
    {
        //Should already be visible but make sure
        DashboardActivityIndicator.IsVisible = true;
        DashboardWebpage.IsVisible = false;
    }

    public void OnNavigated(object sender, WebNavigatedEventArgs e)
    {
        DashboardActivityIndicator.IsVisible = false;
        DashboardWebpage.IsVisible = true;
    }

    public async Task<int?> AddAuditRecord()
    {
        //Add audit record
        AuditService auditService = new AuditService();
        int? auditAdded = await auditService.Add();

        return auditAdded;
    }
}