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
using LiveChartsCore;


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
        Chart.Series = ChartRepo<SecurityModel>.GetSeries(DataRepoProvider.SecurityDatabase.Items, "Loudness");
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
        Actuators.ItemsSource = App.DeviceTwinService.SecurityActuators;
        await App.DeviceTwinService.UpdateActuators();
    }




    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"..//User/Account");
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
}