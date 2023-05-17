using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using Firebase.Auth;
using System.ComponentModel;
using Plugin.LocalNotification;
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
                            Sensor newSensor = new()
                            {
                                Name = propertyInfo.Name,
                                Min = 0,
                                Max = 100,
                                Value = propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null),
                                State = (float)propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null) < 100
                                || (float)propertyInfo.GetValue(DataRepoProvider.SecurityDatabase.LatestItem, null) > 0
                                ? Sensor.SensorState.Valid : Sensor.SensorState.Error
                            };
                            newSensor.PropertyChanged += OnSensorChanged;
                            _sensors.Add(newSensor);
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
    private void OnSensorChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Value")
        {
            Sensor changedSensor = (Sensor)sender;
            if (changedSensor.Value > changedSensor.Max)
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1000,
                    Title = changedSensor.Name + "Sensor passed its max",
                    Subtitle = "Sensor passed its max",
                    Description = "The Sensor has reached its max value",
                    BadgeNumber = 42,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                        NotifyRepeatInterval = TimeSpan.FromDays(1)
                    }
                };
                LocalNotificationCenter.Current.Show(request);
                Console.WriteLine($"Sensor '{changedSensor.Name}' has exceeded its maximum value of '{changedSensor.Max}'");
            }
            else if (changedSensor.Value < changedSensor.Min)
            {
                // create notification for exceeding min value
                Console.WriteLine($"Sensor '{changedSensor.Name}' has fallen below its minimum value of '{changedSensor.Min}'");
            }
        }
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
        try
        {
            foreach (var actuator in _actuators)
            {
                await App.DeviceTwinService.ApplyChanges(actuator);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"Error: Cannot connect to the Iot Hub please check connection", "Ok");
        }
    }


    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//User/Account");
    }

}