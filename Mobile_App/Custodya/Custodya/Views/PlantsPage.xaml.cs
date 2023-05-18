using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;
using System.Reflection;
using Microsoft.Azure.Devices;
<<<<<<< Updated upstream
=======
using Firebase.Auth;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
>>>>>>> Stashed changes

namespace Custodya;

public partial class PlantsPage : ContentPage
{

    private ObservableCollection<Sensor> _sensors = new();
    private ObservableCollection<Actuator> _actuators = new();
    private static RegistryManager registryManager;

    /// <summary>
    /// binds the plant database to its corespiting XMAL part
    /// </summary>
    public PlantsPage()
    {
        registryManager = RegistryManager.CreateFromConnectionString(App.Settings.EventHubConnectionString);
        InitializeComponent();
        this.BindingContext = DataRepoProvider.PlantsDatabase;
<<<<<<< Updated upstream
        HumidityChart.Series = ChartRepo<PlantsModel>.GetSeries(DataRepoProvider.PlantsDatabase.Items, "Humidity");
        TemperatureChart.Series = ChartRepo<PlantsModel>.GetSeries(DataRepoProvider.PlantsDatabase.Items, "Temperature");
        WaterChart.Series = ChartRepo<PlantsModel>.GetSeries(DataRepoProvider.PlantsDatabase.Items, "Water");
        MoistureChart.Series = ChartRepo<PlantsModel>.GetSeries(DataRepoProvider.PlantsDatabase.Items, "Moisture");
=======
        HumidityChart.Series = _humidityChartRepo.DataSeries;
        TemperatureChart.Series = _temperatureChartRepo.DataSeries;
        WaterChart.Series = _waterChartRepo.DataSeries;
        MoistureChart.Series = _moistureChartRepo.DataSeries;

        HumidityChart.XAxes = new List<Axis> { new Axis { Labeler = (value) => new DateTime((long)value).ToString("ddd H:mm"), MinStep = TimeSpan.FromMinutes(2).Ticks, TextSize = 25  } };
        TemperatureChart.XAxes = new List<Axis> { new Axis { Labeler = (value) => new DateTime((long)value).ToString("ddd H:mm"), MinStep = TimeSpan.FromMinutes(2).Ticks, TextSize = 25 } };
        WaterChart.XAxes = new List<Axis> { new Axis { Labeler = (value) => new DateTime((long)value).ToString("ddd H:mm"), MinStep = TimeSpan.FromMinutes(2).Ticks, TextSize = 25  } };
        MoistureChart.XAxes = new List<Axis> { new Axis { Labeler = (value) => new DateTime((long)value).ToString("ddd H:mm"), MinStep = TimeSpan.FromMinutes(2).Ticks, TextSize = 25  } };

        HumidityChart.YAxes = new List<Axis> { new Axis { TextSize = 25 } };
        TemperatureChart.YAxes = new List<Axis> { new Axis { TextSize = 25 } };
        WaterChart.YAxes = new List<Axis> { new Axis { TextSize = 25 } };
        MoistureChart.YAxes = new List<Axis> { new Axis { TextSize = 25 } };
    }
>>>>>>> Stashed changes

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