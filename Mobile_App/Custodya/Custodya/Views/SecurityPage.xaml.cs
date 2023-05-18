using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Custodya;

public partial class SecurityPage : ContentPage
{
    /// <summary>
    /// binds the XAML to the database data, and shows the right user the information that they are intended to see
    /// </summary>
    private ObservableCollection<Sensor> _sensors = new();
    private ObservableCollection<Actuator> _actuators = new();
    private static RegistryManager registryManager;
    private ChartRepo<SecurityModel> _loudnessChart;

    public SecurityPage()
	{
        _loudnessChart = new ChartRepo<SecurityModel>(DataRepoProvider.SecurityDatabase.Items, "Loudness", 20);
        InitializeComponent();
        registryManager = RegistryManager.CreateFromConnectionString(App.Settings.EventHubConnectionString);

        this.BindingContext = DataRepoProvider.SecurityDatabase;
        if (Shell.Current.CurrentItem.Route == "Owner")
        {
            controlFrame.IsVisible = false;
        }
        Chart.Series = _loudnessChart.DataSeries;
        Chart.XAxes = ChartRepo<SecurityModel>.XAxis;
        Chart.YAxes = ChartRepo<SecurityModel>.YAxis;
    }

    private async void toggleState_Toggled(object sender, ToggledEventArgs e)
    {

        var twin = await registryManager.GetTwinAsync(App.Settings.DeviceId);
        Switch switchToggle = (Switch)sender;


        var patch =
        $@"{{
            properties: {{
                desired: {{
                    actuatorControl: {{
                        DoorLock:{{
                            manualState : {switchToggle.IsToggled.ToString().ToLower()}
                        }}
                    }}
                }}
            }}
        }}";
        
        await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);
    }


    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{Shell.Current.CurrentItem.Route}Account");
    }

}