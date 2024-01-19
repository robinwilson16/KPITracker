using DashboardApp.Data;
using DashboardApp.Models;
using DashboardApp.Services;
using DashboardApp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Converters;
using System.Diagnostics;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;

namespace DashboardApp.Pages;

public partial class FooterPartial : ContentView
{
    public FooterPartial()
	{
        InitializeComponent();
        GetConfig();
    }

    public Config? Config { get; set; }
    //private readonly ConfigService _configService;
    //public AttendanceByFacultyPage(ConfigService configService)

    public async void GetConfig()
    {
        //string database = App.Configuration.GetSection("DatabaseConnection")["Database"];

        //ConfigService configService = new ConfigService();
        //Config = await configService.Get("DSH_AcademicYearID");

        if(await App.Context.Database.CanConnectAsync())
        {
            GlobalVariables.IsDbContextValid = true;

            ColorTypeConverter converter = new ColorTypeConverter();
            Color color = (Color)(converter.ConvertFromInvariantString((string)"#1B5E20"));

            ButtonDatabaseStatus.BackgroundColor = color;
            ButtonDatabaseStatus.Text = "O";
        }
        else
        {
            //In case we don't want to keep showing the error message
            if(GlobalVariables.IsDbContextErrorShown == false)
            {
                //Method to show a message box from a view
                await Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    App.AlertSvc.ShowAlert("Alert", "Cannot connect to database", "OK");
                });
                LabelYear.Text = "???";
                ColorTypeConverter converter = new ColorTypeConverter();
                Color color = (Color)(converter.ConvertFromInvariantString((string)"#B71C1C"));

                ButtonDatabaseStatus.BackgroundColor = color;
                ButtonDatabaseStatus.Text = "E";

                //GlobalVariables.IsDbContextErrorShown = true;
            }
            
        }

        try
        {
            while (GlobalVariables.IsDbContextValid == true && GlobalVariables.IsDbContextBusy == true)
            {
                Trace.WriteLine("Database is busy");
                await Task.Delay(25);
            }
            if(GlobalVariables.IsDbContextValid == true)
            {
                GlobalVariables.IsDbContextBusy = true;
                Config = await App.Context.Config.FirstOrDefaultAsync(c => c.ConfigID == "DSH_AcademicYearID");
                GlobalVariables.IsDbContextBusy = false;
                LabelYear.Text = Config.Value;
            }
        }
        catch (Exception ex) {
            //Method to show a message box from a view
            await Task.Run(async () =>
            {
                await Task.Delay(2000);
                App.AlertSvc.ShowAlert("Alert", "Cannot connect to database", "OK");
            });
        }

        //Config = await _context.Config.FirstOrDefaultAsync(c => c.ConfigID == "DSH_AcademicYearID");
        
        //LabelYear.Text = database;


        //Add alerts with feedback
        //await Task.Run(async () =>
        //{
        //    await Task.Delay(2000);
        //    App.AlertSvc.ShowConfirmation("Title", "Confirmation message.", (result =>
        //    {
        //        App.AlertSvc.ShowAlert("Result", $"{result}");
        //    }));
        //});

        if (Environment.UserName is not null)
        {
            LabelUser.Text = Environment.UserName;
        }
        else
        {
            LabelUser.Text = "???";
        }
    }

    public async void ButtonAbout_Clicked(object sender, EventArgs e)
    {
        Window aboutWindow = new Window(new About());
        aboutWindow.Title = "WindowAbout";
        aboutWindow.Width = 500;
        Application.Current?.OpenWindow(aboutWindow);

        //Test function to save a config value
        SettingsService settingsService = new SettingsService();
        Settings? settings = await settingsService.Set();

        //if(Config is not null)
        //{
        //    DatabaseStatus.Text = Config.Value;
        //}
        //else
        //{
        //    DatabaseStatus.Text = "Error";
        //}
    }
}