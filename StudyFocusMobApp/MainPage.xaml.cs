using System.Timers;
using System.Xml.Linq;
using Plugin.Maui.Audio;
using Syncfusion.Maui.Popup;
using Syncfusion.Maui.Sliders;
namespace StudyFocusMobApp;

public partial class MainPage : ContentPage
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private IAudioManager audioManager;
    private int _remainingWorkingTimeInSeconds;//used for both rest time and work time
    private int _workingTimeInSeconds; 
    private bool _isRunning = false;
    private bool _isWorkingCycle = true;
   
    private int _restTimeInSeconds;
    private int _cycleNumber = 3;
    private int _currentCycleNumber;
    private bool _firstRun = true;
    private bool _runFromSlider = false;

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
    public MainPage(IAudioManager audioManager)
    {
        InitializeComponent();
        BindingContext = this;
        this.audioManager = audioManager;
    }


    private async void OnPlayPauseButtonClicked(object sender, EventArgs e)
    {
        if (_firstRun)
        {
            if (int.TryParse(MinutesEntry.Text, out int minutes))
            {
                _workingTimeInSeconds = minutes * 60;
                _remainingWorkingTimeInSeconds = minutes * 60;
                _firstRun = false;
                UpdateCountdownLabel();
            }
            else
            {
                if (!_runFromSlider)
                {
                    _ = DisplayAlert("Error", "Please enter a valid number of minutes.", "OK");
                }
                    
            }
        }
        if (_runFromSlider)
        {
            _firstRun = false;
            UpdateCountdownLabel();
        }
        if (!_isRunning)
        {
            _isRunning = true;
            UpdatePlayPauseButtonSource();
            while (_remainingWorkingTimeInSeconds >= 0 && _isRunning)
            {
                UpdateCountdownLabel();
                await Task.Delay(1000);

                if (!_isRunning)
                {
                    break;
                }
                CalculatePercentage();
                _remainingWorkingTimeInSeconds--;
            }

            if (_remainingWorkingTimeInSeconds <= 0)
            {
                PlayAudioSound();
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
        _remainingWorkingTimeInSeconds = _workingTimeInSeconds;
        CalculatePercentage();
        UpdateCountdownLabel();
        _isRunning = false;
        _firstRun = true;
        UpdatePlayPauseButtonSource();
    }

    private async void PlayAudioSound()
    {
        var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("cycleFinishSound.mp3"));
        player.Play();

        player.Dispose();
    }

    private void UpdateCountdownLabel()
    {
        RemainingTimeLabel.Text = TimeSpan.FromSeconds(_remainingWorkingTimeInSeconds).ToString(@"mm\:ss");
    }

    private void UpdatePlayPauseButtonSource()
    {
        PlayPauseButton.Source = _isRunning ? "ic_pause.png" : "ic_play.png";
    }
    private void CalculatePercentage()
    {
        CoveredTimePercentage = Math.Round(100 - ((double)_remainingWorkingTimeInSeconds / (double)_workingTimeInSeconds) * 100, 2);
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
        _runFromSlider = true;
        int value = (int)e.NewValue;
        _workingTimeInSeconds = value * 60;
        _remainingWorkingTimeInSeconds = value * 60;
    }

    private void restMinuteSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {

    }

    private void cycleSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {

    }
}