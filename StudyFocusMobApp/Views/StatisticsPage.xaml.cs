namespace StudyFocusMobApp;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
        RefreshAppUsageTime();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshAppUsageTime();
        RefreshPomodorosCount();
    }

    private void RefreshPomodorosCount()
    {
        int pomodorosFinished = Preferences.Get("pomodorosFinished", 0);
        // Update the label text
        pomodorosLabel.Text = $"{pomodorosFinished}";
    }

    private void RefreshAppUsageTime()
    {
        if (Preferences.ContainsKey("AppUsageTime"))
        {
            int totalSeconds = Preferences.Get("AppUsageTime", 0);
            TimeSpan appUsageTime = TimeSpan.FromSeconds(totalSeconds);

            // Calculate hours, minutes, and seconds
            int hours = (int)appUsageTime.TotalHours;
            int minutes = appUsageTime.Minutes;
            int seconds = appUsageTime.Seconds;

            // Update the label text
            appUsageLabel.Text = $"{hours} hours, {minutes} minutes, {seconds} seconds";
        }
    }
}