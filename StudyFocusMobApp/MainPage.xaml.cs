using System.Timers;
using System.Xml.Linq;
using Syncfusion.Maui.Popup;
using Syncfusion.Maui.Sliders;
namespace StudyFocusMobApp;

public partial class MainPage : ContentPage
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private int _remainingTimeInSeconds = 10;
    private int _timeInSeconds = 10;
    private bool _isRunning;
    private double _coveredTimePercentage = 0.0;
    public double CoveredTimePercentage
    {
        get { return _coveredTimePercentage; }
        set
        {
            _coveredTimePercentage = value;
            OnPropertyChanged();
        }
    }

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnPlayPauseButtonClicked(object sender, EventArgs e)
    {
        if (!_isRunning)
        {
            _isRunning = true;
            UpdatePlayPauseButtonSource();
            while (_remainingTimeInSeconds >= 0 && _isRunning)
            {
                UpdateCountdownLabel();
                await Task.Delay(1000);

                if (!_isRunning)
                {
                    break;
                }
                CalculatePercentage();
                _remainingTimeInSeconds--;
            }

            if (_remainingTimeInSeconds <= 0)
            {
                _cancellationTokenSource?.Cancel();
            }

            _isRunning = false;
            UpdatePlayPauseButtonSource();
        }
        else
        {
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            UpdatePlayPauseButtonSource();
        }
    }

    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel();
        _remainingTimeInSeconds = _timeInSeconds;
        CalculatePercentage();
        UpdateCountdownLabel();
        _isRunning = false;
        UpdatePlayPauseButtonSource();
    }

    private void UpdateCountdownLabel()
    {
        RemainingTimeLabel.Text = TimeSpan.FromSeconds(_remainingTimeInSeconds).ToString(@"mm\:ss");
    }

    private void UpdatePlayPauseButtonSource()
    {
        PlayPauseButton.Source = _isRunning ? "ic_pause.png" : "ic_play.png";
    }

    private void CalculatePercentage()
    {
        CoveredTimePercentage = Math.Round(100 - ((double)_remainingTimeInSeconds / (double)_timeInSeconds) * 100, 2);
    }

    private async void SettingsButton_Clicked(object sender, EventArgs e)
    {
        if (_isRunning)
        {
            await DisplayAlert("Alert!", "Stop your current timer first", "OK");
        }
        else
        {
            settingsPopup.Show();
        }
    }

    private void workMinuteSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {
        int value = (int)e.NewValue;
        _timeInSeconds = value * 60;
        _remainingTimeInSeconds = value * 60;
    }

    private void restMinuteSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {

    }

    private void cycleSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {

    }

    private void settingsPopup_Closed(object sender, EventArgs e)
    {
        UpdateCountdownLabel();
    }
}
