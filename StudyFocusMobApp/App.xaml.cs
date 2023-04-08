namespace StudyFocusMobApp;

public partial class App : Application
{
	public App()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTY2MDQyNEAzMjMxMmUzMTJlMzMzNW9US2g5R1oxZ1pnN09VMVpSUHF6VmxNRGd5UGFnaTdwUlZ5Y3oycDNnRjg9");

        InitializeComponent();

		MainPage = new AppShell();
	}
}
