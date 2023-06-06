using Plugin.LocalNotification;
using Syncfusion.Maui.Sliders;
using CommunityToolkit.Maui.Alerts;
using NotificationManager = StudyFocusMobApp.Services.NotificationManager;
using Plugin.Maui.Audio;

namespace StudyFocusMobApp;

public partial class MainPage : ContentPage
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    // Fields initialized to set up clock in case when user does not click settings and instantly clicks play after the boot
    private int _remainingTimeInSeconds = 1800; // used for both work and rest for main clock
    private int _workingTimeInSeconds = 1800;
    private int _restTimeInSeconds = 300;
    private int _cycleNumber = 3;
    private int _cyclesRemaining = 3;
    private bool _isRunning = false;
    private bool _isWorkingCycle = false;
    private bool _isPaused = false;
    private double _coveredTimePercentage = 0.0; // field bound to the clock
    private bool notificationsVisible = true;
    private readonly IAudioManager audioManager;
    private IAudioPlayer audioPlayer;
    private DateTime appStartTime;
    
    // Property for the covered time percentage
    public double CoveredTimePercentage
    {
        get { return _coveredTimePercentage; }
        set
        {
            _coveredTimePercentage = value;
            OnPropertyChanged();
        }
    }

    public MainPage(IAudioManager audioManager)
    {
        InitializeComponent();
        appStartTime = DateTime.Now;
        this.audioManager = audioManager;
        BindingContext = this;
        LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Calculate app usage time and store it in preferences
        TimeSpan appUsageTime = DateTime.Now - appStartTime;
        appStartTime = DateTime.Now;
        int totalSeconds = (int)appUsageTime.TotalSeconds;

        // Retrieve the current value from preferences
        int existingAppUsageTime = Preferences.Get("AppUsageTime", 0);

        // Add the new app usage time to the existing value
        int updatedAppUsageTime = existingAppUsageTime + totalSeconds;

        // Store the updated value in preferences
        Preferences.Set("AppUsageTime", updatedAppUsageTime);
    }

    private async void OnPlayPauseButtonClicked(object sender, EventArgs e)
    {
        if (!_isRunning)
        {
            // Start the timer if it's not running
            _isRunning = true;
            _isWorkingCycle = true;
            UpdatePlayPauseButtonSource();

            while (_cyclesRemaining > 0 && _isRunning)
            {
                CycleLabel.Text = "Cycle № " + (_cycleNumber - _cyclesRemaining + 1).ToString();
                if (!_isPaused)
                {
                    // Set the remaining time based on the current cycle
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
                
                // Switch between work and rest cycles
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
                    // Finish the timer cycle
                    _cancellationTokenSource?.Cancel();
                    PomodorosCount();
                }
            }
            
            //Reset the timer and update the UI
            _isRunning = false;
            UpdatePlayPauseButtonSource();
        }
        else
        {
            // Pause the timer if it's running
            _isPaused = true;
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            UpdatePlayPauseButtonSource();
        }
    }

    private void PomodorosCount()
    {
        // Retrieve the current value from preferences
        int existingPomodorosFinished = Preferences.Get("pomodorosFinished", 0);

        // Add 1 more cycle to the count
        int updatedAppUsageTime = existingPomodorosFinished + 1;

        // Store the updated value in preferences
        Preferences.Set("AppUsageTime", updatedAppUsageTime);
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
        //Helper method used to convert the time to percentage   
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
        // Updates the working time based on the slider value change
        int value = (int)e.NewValue;
        _workingTimeInSeconds = value * 60;
        _remainingTimeInSeconds = value * 60;
    }

    private void restMinuteSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {
        // Updates the rest time based on the slider value change
        int value = (int)e.NewValue;
        _restTimeInSeconds = value * 60;
    }

    private void cycleSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {
        // Updates the cycle number based on the slider value change
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
            // Handle the notification dismissal action
        }
        else if (e.IsTapped)
        {
            // Handle the notification tap action
        }
    }

    private void notificationButton_Clicked(object sender, EventArgs e)
    {
        if (notificationsVisible)
        {
            new NotificationManager().HideAllNotifications();
            var toast = Toast.Make("Notifications Off", CommunityToolkit.Maui.Core.ToastDuration.Short, 18);
            toast.Show();
        }
        else
        {
            var toast = Toast.Make("Notifications On", CommunityToolkit.Maui.Core.ToastDuration.Short, 18);
            toast.Show();
        }

        notificationsVisible = !notificationsVisible;

    }

    private async void musicButton_Clicked(object sender, EventArgs e)
    {
        if (audioPlayer == null)
        {
            // Create a new player if it hasn't been initialized yet
            var audioFile = await FileSystem.OpenAppPackageFileAsync("lofi.mp3");
            audioPlayer = audioManager.CreatePlayer(audioFile);
            audioPlayer.Play();
        }
        else
        {
            // Pause or resume the audio player based on its current state
            if (audioPlayer.IsPlaying)
            {
                audioPlayer.Pause();
            }
            else
            {
                audioPlayer.Play();
            }
        }
    }
    private void ApplicationClosing(object sender, EventArgs e)
    {
        //Dispose the audio player when the application is closing
        audioPlayer?.Dispose();
    }
}
