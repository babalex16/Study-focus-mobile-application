using Plugin.LocalNotification;
using Syncfusion.Maui.Core.Hosting;

namespace StudyFocusMobApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Lobster-Regular.ttf", "Lobster-Regular");

            });
        builder.ConfigureSyncfusionCore();

        return builder.Build();
	}
}
