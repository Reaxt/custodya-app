using Microsoft.Extensions.Configuration;
using Custodya.Config;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Custodya.Services;
using Custodya.Models;
using Custodya.Interfaces;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;

namespace Custodya;

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
            })
            .RegisterCustodyaServices()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit();


#if DEBUG
        builder.Logging.AddDebug();
#endif
        //NEW CODE: before the return statement...
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream($"{a.GetName().Name}.Config.appsettings.json");
        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
        builder.Configuration.AddConfiguration(config);
        var app = builder.Build();
        Services = app.Services;
        return app;

    }
    public static IServiceProvider Services { get; private set; }

    public static MauiAppBuilder RegisterCustodyaServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IGenericRealtimeDatabase<PlantsModel>, FirebaseRealtimeDatabaseService<PlantsModel>>();
        builder.Services.AddSingleton<IGenericRealtimeDatabase<GeoLocationModel>, FirebaseRealtimeDatabaseService<GeoLocationModel>>();
        builder.Services.AddSingleton<IGenericRealtimeDatabase<SecurityModel>, FirebaseRealtimeDatabaseService<SecurityModel>>();
        builder.Services.AddSingleton<IGenericDatabase<GeoLocationModel>>(x => x.GetService<IGenericRealtimeDatabase<GeoLocationModel>>());
        builder.Services.AddSingleton<IGenericDatabase<SecurityModel>>(x => x.GetService<IGenericRealtimeDatabase<SecurityModel>>());
        builder.Services.AddSingleton<IGenericDatabase<SecurityModel>>(x => x.GetService<IGenericRealtimeDatabase<SecurityModel>>());
        builder.Services.AddSingleton<EventHubService>();
        builder.Services.AddSingleton<DeviceTwinService>();
        builder.Services.AddSingleton<ErrorAlertProviderService>();
        return builder;
    }
}
