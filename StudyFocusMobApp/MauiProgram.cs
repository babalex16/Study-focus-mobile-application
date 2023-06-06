using Plugin.LocalNotification;
using StudyFocusMobApp.Services;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui;
using Plugin.Maui.Audio;

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
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        builder.ConfigureSyncfusionCore();

        string dbPath = FileAccessHelper.GetLocalFilePath("todolist.db3");
        builder.Services.AddSingleton<TodoService>(s => ActivatorUtilities.CreateInstance<TodoService>(s, dbPath));

        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddTransient<MainPage>();
        return builder.Build();
    }
}
