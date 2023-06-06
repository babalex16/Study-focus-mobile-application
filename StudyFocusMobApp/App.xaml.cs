using StudyFocusMobApp.Services;

namespace StudyFocusMobApp;

public partial class App : Application
{
    public static TodoService TodoSvc { get; private set; }
    public App(TodoService srv)
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTY2MDQyNEAzMjMxMmUzMTJlMzMzNW9US2g5R1oxZ1pnN09VMVpSUHF6VmxNRGd5UGFnaTdwUlZ5Y3oycDNnRjg9");

        InitializeComponent();

        MainPage = new AppShell();

        TodoSvc = srv;
    }
}
