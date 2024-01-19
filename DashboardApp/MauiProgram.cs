using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DashboardApp.Data;
using System.Reflection;
using Microsoft.Maui.LifecycleEvents;
using DashboardApp.Interfaces;
using Microsoft.Data.SqlClient;
#if WINDOWS
using DashboardApp.Platforms.Windows;
#endif

namespace DashboardApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiAppAsync()
        {
            var builder = MauiApp.CreateBuilder();

            //Add ability to show alerts from view models
            builder.Services.AddSingleton<IAlertService, AlertService>();

            var a = Assembly.GetExecutingAssembly();
            using Stream? stream = a.GetManifestResourceStream("DashboardApp.appsettings.json");
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
            builder.Configuration.AddConfiguration(config);

            //Get connection string elements to assemble
            var databaseSettings = config.GetSection("DatabaseConnection");
            string server = databaseSettings["server"];
            string database = databaseSettings["database"];
            string username = databaseSettings["username"];
            string password = databaseSettings["password"];

            var conStrBuilder = new SqlConnectionStringBuilder(
                builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
            
            conStrBuilder.DataSource = server;
            conStrBuilder.InitialCatalog = database;
            conStrBuilder.UserID = username;
            conStrBuilder.Password = password;
            var connectionString = conStrBuilder.ConnectionString;

            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            //Add Debuging to output with Trace.WriteLine
            builder.Logging.AddDebug();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string versionNumberConfig = config["version"];
            string versionNumberApp = AppInfo.Current.VersionString;
            string buildNumberApp = AppInfo.Current.BuildString;
            if(versionNumberApp.Length > 4)
            {
                versionNumberApp = versionNumberApp.Substring(0, 4) + buildNumberApp;
            }

#if WINDOWS
            TitleContainer.SetTitle("KPI Tracker (" + versionNumberApp + ")");
            //TitleContainer.SetTitlebarColor(Colors.Yellow);
#endif
            
            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        if (window.Title== "WindowMain")
                        {
                            //get the Primary window title named "WindowMain"
                            var windows = window;
                            //use Microsoft.UI.Windowing functions for window
                            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                           //When user execute the closing method, we can push a display alert. If user click Yes, close this application, if click the cancel, display alert will dismiss.
                            appWindow.Closing += async (s, e) =>
                            {
                                e.Cancel = true;
                                //get all of Microsoft.Maui.Controls.windows.
                                var windows1 = Application.Current.Windows.ToList<Microsoft.Maui.Controls.Window>();
                                foreach (Window win in windows1) {
                                    //get the Primary window title named "WindowMain"
                                    if (win.Title== "WindowMain")
                                    {                    
                                        bool result = await win.Page.DisplayAlert(
                                        "Are You Sure?",
                                        "You sure want to close the KPI Dashboard?",
                                        "Yes",
                                        "Cancel");
                                       if (result)
                                        {
                                            //Application.Current.CloseWindow(win);
                                            //Close all windows
                                            Application.Current.Quit();
                                        }
                                    }
                                }

                           };
                        }
      
                    });
                });
#endif

            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        //static async Task<string> LoadMauiAsset()
        //{
        //    try
        //    {
        //        using var stream = await FileSystem.OpenAppPackageFileAsync("appsettings.json");
        //        using var reader = new StreamReader(stream);

        //        return reader.ReadToEnd();
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }

        //}
    }
}
