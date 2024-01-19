using DashboardApp.Models;
using DashboardApp.Services;
using Microsoft.Extensions.Configuration;

namespace DashboardApp.Pages
{
    public partial class RetentionByFaculty : ContentPage
    {
        public Config? Config { get; set; }
        public RetentionByFaculty()
        {
            InitializeComponent();
            LoadDashboard("RetentionByFaculty");
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
    }

}
