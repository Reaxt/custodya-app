using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;
using System.Reflection;

namespace Custodya;

public partial class PlantsPage : ContentPage
{
    private ObservableCollection<Sensor> _sensors = new();
    private ObservableCollection<Actuator> _actuators = new();
    public PlantsPage()
    {
        InitializeComponent();
        this.BindingContext = DataRepoProvider.PlantsDatabase;
        try
        {
            foreach (PropertyInfo propertyInfo in DataRepoProvider.PlantsDatabase.LatestItem.GetType().GetProperties())
            {
                try
                {
                    if (Sensor.PlantSensors.Contains(propertyInfo.Name) && _sensors != null && !_sensors.Any(s => s.Name == propertyInfo.Name))
                    {
                        if (propertyInfo.PropertyType == typeof(float))
                        {
                            _sensors.Add(new()
                            {
                                Name = propertyInfo.Name,
                                Value = propertyInfo.GetValue(DataRepoProvider.PlantsDatabase.LatestItem, null),
                                State = (float)propertyInfo.GetValue(DataRepoProvider.PlantsDatabase.LatestItem, null) < 100
                                || (float)propertyInfo.GetValue(DataRepoProvider.PlantsDatabase.LatestItem, null) > 0
                                ? Sensor.SensorState.Valid : Sensor.SensorState.Error
                            });
                        }
                    }
                    else if (Actuator.PlantActuators.Contains(propertyInfo.Name) && !_actuators.Any(a => a.Name == propertyInfo.Name))
                    {
                        _actuators.Add(new()
                        {
                            Name = propertyInfo.Name,
                            State = (bool)propertyInfo.GetValue(DataRepoProvider.PlantsDatabase.LatestItem, null)
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

    private void toggleState_Toggled(object sender, ToggledEventArgs e)
    {
        // Logic goes here
    }
    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{Shell.Current.CurrentItem.Route}/Account");
    }
}