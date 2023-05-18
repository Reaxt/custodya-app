using Microsoft.Extensions.Configuration;
using Custodya.Config;
using Custodya.Services;

namespace Custodya;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
    public static Settings Settings { get; private set; } = MauiProgram.Services.GetService<IConfiguration>() //gets the required connection strings
	.GetRequiredSection(nameof(Settings)).Get<Settings>();
	public static EventHubService EventHubService { get { return MauiProgram.Services.GetService<EventHubService>(); } } 
	public static DeviceTwinService DeviceTwinService { get { return MauiProgram.Services.GetService<DeviceTwinService>(); } }
}
