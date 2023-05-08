using Microsoft.Extensions.Configuration;
using Custodya.Config;

namespace Custodya;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
    public static Settings Settings { get; private set; } = MauiProgram.Services.GetService<IConfiguration>()
	.GetRequiredSection(nameof(Settings)).Get<Settings>();
}
