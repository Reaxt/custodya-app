using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;
using System.Reflection;
using Microsoft.Azure.Devices;
using Firebase.Auth;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Custodya;

public partial class PlantsPage : ContentPage
{

    private ObservableCollection<Sensor> _sensors = new();
    private ObservableCollection<Actuator> _actuators = new();
    private static RegistryManager registryManager;
    private ChartRepo<PlantsModel> _humidityChartRepo;
    private ChartRepo<PlantsModel> _moistureChartRepo;
    private ChartRepo<PlantsModel> _waterChartRepo;
    private ChartRepo<PlantsModel> _temperatureChartRepo;

    /// <summary>
    /// binds the plant database to its corespiting XMAL part
    /// </summary>
    public PlantsPage()
    {

        _humidityChartRepo = new ChartRepo<PlantsModel>(DataRepoProvider.PlantsDatabase.Items, "Humidity", 20);
        _moistureChartRepo = new ChartRepo<PlantsModel>(DataRepoProvider.PlantsDatabase.Items, "Moisture", 20);
        _waterChartRepo = new ChartRepo<PlantsModel>(DataRepoProvider.PlantsDatabase.Items, "Water", 20);
        _temperatureChartRepo = new ChartRepo<PlantsModel>(DataRepoProvider.PlantsDatabase.Items, "Temperature", 20);
        registryManager = RegistryManager.CreateFromConnectionString(App.Settings.EventHubConnectionString);

        InitializeComponent();
        this.BindingContext = DataRepoProvider.PlantsDatabase;

        HumidityChart.Series = _humidityChartRepo.DataSeries;
        TemperatureChart.Series = _temperatureChartRepo.DataSeries;
        WaterChart.Series = _waterChartRepo.DataSeries;
        MoistureChart.Series = _moistureChartRepo.DataSeries;

        HumidityChart.XAxes = TemperatureChart.XAxes = WaterChart.XAxes = MoistureChart.XAxes = ChartRepo<PlantsModel>.XAxis;
        HumidityChart.YAxes = TemperatureChart.YAxes = WaterChart.YAxes = MoistureChart.YAxes = ChartRepo<PlantsModel>.YAxis;
    }


    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{Shell.Current.CurrentItem.Route}Account");
    }

    private async void toggleLedState_Toggled(object sender, ToggledEventArgs e)
    {
        var twin = await registryManager.GetTwinAsync(App.Settings.DeviceId);
        Switch switchToggle = (Switch)sender;
        var patch =
        $@"{{
            properties: {{
                desired: {{
                    actuatorControl: {{
                        Led:{{
                            manualState : {switchToggle.IsToggled.ToString().ToLower()}
                        }}
                    }}
                }}
            }}
        }}
        ";

        await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);
    }

    private async void toggleFanState_Toggled(object sender, ToggledEventArgs e)
    {
        var twin = await registryManager.GetTwinAsync(App.Settings.DeviceId);
        Switch switchToggle = (Switch)sender;
        var patch =
        $@"{{
            properties: {{
                desired: {{
                    actuatorControl: {{
                        Fan:{{
                            manualState : {switchToggle.IsToggled.ToString().ToLower()}
                        }}
                    }}
                }}
            }}
        }}
        ";

        await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);
    }
}