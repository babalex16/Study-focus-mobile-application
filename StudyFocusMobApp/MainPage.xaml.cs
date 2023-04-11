using System.Timers;
using System.Xml.Linq;
using Syncfusion.Maui.Popup;
using Syncfusion.Maui.Sliders;
namespace StudyFocusMobApp;

public partial class MainPage : ContentPage
{
    private int timeInSeconds = 1200;
    private int remainingTimeInSeconds = 1200;
    private double coveredTimePercentage = 0.0;
    public double CoveredTimePercentage
    {
        get { return this.coveredTimePercentage; } 
        set 
        { 
            this.coveredTimePercentage = value;
            OnPropertyChanged();
        }
    }
    public MainPage()
	{
        InitializeComponent();
        BindingContext = this;
    }

    private void StartButton_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(MinutesEntry.Text, out int minutes))
        {
            timeInSeconds = minutes * 60;
            remainingTimeInSeconds = minutes * 60;
            CountDown();
        }
        else
        {
            DisplayAlert("Error", "Please enter a valid number of minutes.", "OK");
        }
    }
    private async void CountDown()
    {
        while (remainingTimeInSeconds > 0)
        {
            //RemainingTimeLabel.Text = TimeSpan.FromSeconds(remainingTimeInSeconds).ToString(@"mm\:ss");
            RemainingTimeLabel.Text = CoveredTimePercentage.ToString();
            await Task.Delay(1000);
            remainingTimeInSeconds--;
            CalculatePercentage();
        }
        RemainingTimeLabel.Text = "00:00";
    }

    private void StopButton_Clicked(object sender, EventArgs e)
    {
        remainingTimeInSeconds = 0;
    }

    private void SettingsButton_Clicked(object sender, EventArgs e)
    {
        popup.Show();
    }
    private void CalculatePercentage() 
    {
        CoveredTimePercentage = Math.Round(100 - ((double)remainingTimeInSeconds / (double)timeInSeconds)*100, 2);
    }
}

