using System.Timers;

namespace StudyFocusMobApp;

public partial class MainPage : ContentPage
{
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

}

