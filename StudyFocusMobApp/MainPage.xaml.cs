using System.Timers;
using System.Xml.Linq;
using Syncfusion.Maui.Popup;
namespace StudyFocusMobApp;

public partial class MainPage : ContentPage
{
    private int remainingTimeInSeconds;
    SfPopup popup = new SfPopup();
    public MainPage()
	{
        InitializeComponent();
        var timer = new System.Timers.Timer(1000);
        timer.Elapsed += new ElapsedEventHandler(RedrawClock);
        timer.Start();
    }

    public void RedrawClock(object source, ElapsedEventArgs e)
    {
        var graphicsView = this.RadialTimerGraphicsView;

        graphicsView.Invalidate();
    }

    private void StartButton_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(MinutesEntry.Text, out int minutes))
        {
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
            RemainingTimeLabel.Text = TimeSpan.FromSeconds(remainingTimeInSeconds).ToString(@"mm\:ss");
            await Task.Delay(1000);
            remainingTimeInSeconds--;
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
}

