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

        try
        {
            foreach (PropertyInfo propertyInfo in DataRepoProvider.GeolocationDatabase.LatestItem.GetType().GetProperties())
            {
                try
                {
                    if (Sensor.GeoSensors.Contains(propertyInfo.Name) && _sensors != null && !_sensors.Any(s => s.Name == propertyInfo.Name))
                    {
                        if (propertyInfo.PropertyType == typeof(string) && !propertyInfo.Name.Contains("Json"))
                        {
                            _sensors.Add(new()
                            {
                                Name = propertyInfo.Name,
                                Value = propertyInfo.GetValue(DataRepoProvider.GeolocationDatabase.LatestItem, null),
                                State = Sensor.SensorState.Valid,
                                Editable = false
                            });
                        }
                        else if (propertyInfo.PropertyType == typeof(bool))
                        {
                            _sensors.Add(new()
                            {
                                Name = propertyInfo.Name,
                                Value = propertyInfo.GetValue(DataRepoProvider.GeolocationDatabase.LatestItem, null),
                                State = (bool)propertyInfo.GetValue(DataRepoProvider.GeolocationDatabase.LatestItem, null) ? Sensor.SensorState.Error : Sensor.SensorState.Valid,
                                Editable = false
                            });
                        }
                    }
                    else if (Actuator.GeoActuators.Contains(propertyInfo.Name) && !_actuators.Any(a => a.Name == propertyInfo.Name))
                    {
                        _actuators.Add(new()
                        {
                            Name = propertyInfo.Name,
                            State = (bool)propertyInfo.GetValue(DataRepoProvider.GeolocationDatabase.LatestItem, null)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Sensors.ItemsSource = _sensors;
        Actuators.ItemsSource = _actuators;
    }
    private async void ibtnEditSensor_Clicked(object sender, EventArgs e)
    {
        try
        {
            ImageButton btn = (ImageButton)sender;
            Sensor s = (Sensor)btn.BindingContext;
            do
            {
                s.Min = double.Parse(await DisplayPromptAsync("Min", $"Please input a minimum value below the current max: {s.Max}", "Ok", "Cancel", null, 10, Keyboard.Numeric));
            }
            while (s.Min >= s.Max);

            do
            {
                s.Max = double.Parse(await DisplayPromptAsync("Max", $"Please input a maximum value above the current min: {s.Min}", "Ok", "Cancel", null, 10, Keyboard.Numeric));
            }
            while (s.Max <= s.Min);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async void toggleState_Toggled(object sender, ToggledEventArgs e)
    {
        var twin = await registryManager.GetTwinAsync(App.Settings.HubConnectionString);
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
        await Shell.Current.GoToAsync($"//{Shell.Current.CurrentItem.Route}/Account");
    }
}