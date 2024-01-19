using DashboardApp.Data;
using DashboardApp.Interfaces;
using DashboardApp.Models;
using DashboardApp.Services;
using Microsoft.Extensions.Configuration;

namespace DashboardApp
{
    public partial class App : Application
    {
        public static IServiceProvider Services;
        public static IAlertService AlertSvc;
        public static ApplicationDbContext Context;
        public static IConfiguration Configuration;

        public App(ApplicationDbContext context, IServiceProvider provider, IConfiguration configuration)
        {
            InitializeComponent();

            //Add ability to show alerts from view models
            Services = provider;
            AlertSvc = Services.GetService<IAlertService>();
            Context = context;
            Configuration = configuration;

            MainPage = new AppShell();
        }

        private bool IsWindows()
        {
            return DeviceInfo.Current.Platform == DevicePlatform.WinUI;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);
            window.Title = "WindowMain";

            //If device platform is Windows then show dashboard as a side panel on the right
            if (IsWindows() == true)
            {
                // Get display size
                var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
                double taskbarHeight = 40;
                double screenGap = 10;

                // Set window size
                window.Width = 410;
                window.Height = (displayInfo.Height / displayInfo.Density) - taskbarHeight - (screenGap * 2);

                //Calculate positions
                double horizontalPosCentre = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
                double verticalPosCentre = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
                double horizontalPosRight = (displayInfo.Width / displayInfo.Density - window.Width) - screenGap;
                double verticalPosRight = screenGap;

                //Set window position
                window.X = horizontalPosRight;
                window.Y = verticalPosRight;
            };

            return window;
        }
    }
}
