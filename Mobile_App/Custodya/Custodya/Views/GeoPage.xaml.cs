using Custodya.Models;
using Custodya.Repos;
using Microsoft.Azure.Devices;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Custodya;

/// <summary>
/// bindingContext for the GeoLocation DataBase
/// </summary>
public partial class GeoPage : ContentPage
{
    private ObservableCollection<Sensor> _sensors = new();
    private ObservableCollection<Actuator> _actuators = new();
    private static RegistryManager registryManager;

    public GeoPage()
    {
        InitializeComponent();
        this.BindingContext = DataRepoProvider.GeolocationDatabase;
        registryManager = RegistryManager.CreateFromConnectionString(App.Settings.EventHubConnectionString);
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
                        Buzzer:{{
                            manualState : {switchToggle.IsToggled}
                        }}
                    }}
                }}
            }}
        }}
        ";

        await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);
    }

    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{Shell.Current.CurrentItem.Route}/Account", true);
    }
}