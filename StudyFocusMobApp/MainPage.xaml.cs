using System.Timers;
using System.Xml.Linq;
using Plugin.LocalNotification;
using Syncfusion.Maui.Popup;
using Syncfusion.Maui.Sliders;
namespace StudyFocusMobApp;

public partial class MainPage : ContentPage
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    // fields initialized to set up clock in case when user does not click settings and instantly clicks play after the boot
    private int _remainingTimeInSeconds = 20; // used for both work and rest for main clock
    private int _workingTimeInSeconds = 20;
    private int _restTimeInSeconds = 10;
    private int _cycleNumber = 3;
    private int _cyclesRemaining = 3;
    private bool _isRunning = false;
    private bool _isWorkingCycle = false;
    private bool _isPaused = false;
    private double _coveredTimePercentage = 0.0; // field binded to the graphical view
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
        LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
    }

    private async void OnPlayPauseButtonClicked(object sender, EventArgs e)
    {
        if (!_isRunning)
        {
            _isRunning = true;
            _isWorkingCycle = true;
            UpdatePlayPauseButtonSource();

            while (_cyclesRemaining > 0 && _isRunning)
            {
                CycleLabel.Text = "Cycle № " + (_cycleNumber - _cyclesRemaining + 1).ToString();
                if (!_isPaused)
                {
                    if (_isWorkingCycle)
                    {
                        _remainingTimeInSeconds = _workingTimeInSeconds;
                    }
                    else
                    {
                        _remainingTimeInSeconds = _restTimeInSeconds;
                    }
                }
                _isPaused = false;
                while (_remainingTimeInSeconds >= 0 && _isRunning)
                {
                    if (_isWorkingCycle)
                    {
                        CalculatePercentage(_workingTimeInSeconds);
                    }
                    else
                    {
                        CalculatePercentage(_restTimeInSeconds);
                    }
                    if (_remainingTimeInSeconds <= 0)
                    {
                        UpdateCountdownLabel();
                        SendNotification();
                        await Task.Delay(3000); // Delay between work - rest cycle transitions
                    }
                    UpdateCountdownLabel();
                    await Task.Delay(1000);

                    if (!_isRunning)
                    {
                        break;
                    }
                    _remainingTimeInSeconds--;
                }

                if (_isWorkingCycle)
                {
                    _isWorkingCycle = false;
                }
                else
                {
                    _cyclesRemaining--;
                    _isWorkingCycle = true;
                }

                if (_cyclesRemaining <= 0)
                {
                    _cancellationTokenSource?.Cancel();
                }
            }

            _isRunning = false;
            UpdatePlayPauseButtonSource();
        }
        else
        {
            _isPaused = true;
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            UpdatePlayPauseButtonSource();
        }
    }

    private void SendNotification()
    {
        var request = new NotificationRequest
        {
            NotificationId = 1337,
            Title = "Timer's up!",
            Description = "Come back ^_^",
            BadgeNumber = 42,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(1)
            }
        };

        LocalNotificationCenter.Current.Show(request);
    }

    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel();
        _remainingTimeInSeconds = _workingTimeInSeconds;
        _cyclesRemaining = _cycleNumber;
        CalculatePercentage(_workingTimeInSeconds);
        _isRunning = false;
        CycleLabel.Text = "Cycle № 1";
        UpdateCountdownLabel();
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

    private void CalculatePercentage(int time)
    {
        CoveredTimePercentage = Math.Round(100 - ((double)_remainingTimeInSeconds / (double)time) * 100, 2);
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
        _workingTimeInSeconds = value * 60;
        _remainingTimeInSeconds = value * 60;
    }

    private void restMinuteSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {
        int value = (int)e.NewValue;
        _restTimeInSeconds = value * 60;
    }

    private void cycleSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {
        int value = (int)e.NewValue;
        _cycleNumber = value;
        _cyclesRemaining = value;
    }

    private void settingsPopup_Closed(object sender, EventArgs e)
    {
        CalculatePercentage(_workingTimeInSeconds);
        UpdateCountdownLabel();
    }

    private void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
    {
        if (e.IsDismissed)
        {

        }
        else if (e.IsTapped)
        {

        }
    }
}
