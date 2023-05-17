using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using Custodya.Services;

namespace Custodya;

public partial class SecurityPage : ContentPage
{
    /// <summary>
    /// binds the XAML to the database data, and shows the right user the information that they are intended to see
    /// </summary>
    private ObservableCollection<Sensor> _sensors = new();
    private ObservableCollection<Actuator> _actuators = new();
    private static RegistryManager registryManager;
    

    public SecurityPage()
	{
        InitializeComponent();
        registryManager = RegistryManager.CreateFromConnectionString(App.Settings.EventHubConnectionString);

        this.BindingContext = DataRepoProvider.SecurityDatabase;
        if (Shell.Current.CurrentItem.Route == "Owner")
        {
            controlFrame.IsVisible = false;
        }
        try
        {
            foreach (PropertyInfo propertyInfo in DataRepoProvider.SecurityDatabase.LatestItem.GetType().GetProperties())
            {
                try
                {
                    if (Sensor.SecuritySensors.Contains(propertyInfo.Name) && _sensors != null && !_sensors.Any(s => s.Name == propertyInfo.Name))
                    {
                        if (propertyInfo.PropertyType == typeof(float))
                        {
                            _sensors.Add(new()
                            {
                                Name = propertyInfo.Name,
                                Min = 0,
                                Max = 100,
                                Value = propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null),
                                State = (float)propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null) < 100
                                || (float)propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null) > 0
                                ? Sensor.SensorState.Valid : Sensor.SensorState.Error
                            });
                        }
                        else
                        {
                            if(propertyInfo.PropertyType == typeof(bool))
                            {
                                _sensors.Add(new()
                                {
                                    Name = propertyInfo.Name,
                                    Value = propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null),
                                    State = (bool) propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null) ? Sensor.SensorState.Error : Sensor.SensorState.Valid,
                                    Editable = false
                                });
                            }
                            else if (propertyInfo.PropertyType == typeof(SecurityModel.DoorState))
                            {
                                _sensors.Add(new()
                                {
                                    Name = propertyInfo.Name,
                                    Value = propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null),
                                    State = (SecurityModel.DoorState)propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null) == SecurityModel.DoorState.Closed ? Sensor.SensorState.Error : Sensor.SensorState.Valid,
                                    Editable = false
                                });
                            }
                            
                        }
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
    protected override async void OnAppearing()
    {
        UpdateActuators();
    }
    private async void UpdateActuators()
    {
        var actuators = await App.DeviceTwinService.GetActuators(Actuator.SecurityActuators);
        foreach (var actuator in actuators)
        {
            if (_actuators.Any(x => x.Name == actuator.Name))
            {
                int index = _actuators.IndexOf(_actuators.First(x => x.Name == actuator.Name));
                _actuators[index] = actuator;
            }
            else
            {
                _actuators.Add(actuator);
            }
        }
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
        Actuator test = sender as Actuator;
        foreach (var actuator in _actuators)
        {
            await App.DeviceTwinService.ApplyChanges(actuator);
        }
    }


    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//User/Account");
    }

}