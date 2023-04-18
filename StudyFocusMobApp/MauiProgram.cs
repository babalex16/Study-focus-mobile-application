using Plugin.Maui.Audio;
using Syncfusion.Maui.Core.Hosting;

namespace StudyFocusMobApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.ConfigureSyncfusionCore();
		builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddTransient<MainPage>();	

        return builder.Build();
	}
}
