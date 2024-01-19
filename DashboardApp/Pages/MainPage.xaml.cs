using Microsoft.Extensions.Configuration;

namespace DashboardApp.Pages
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        public void ButtonAbout_Clicked(object sender, EventArgs e)
        {
            Window secondWindow = new Window(new MainPage());
            Application.Current?.OpenWindow(secondWindow);
        }
    }

}
