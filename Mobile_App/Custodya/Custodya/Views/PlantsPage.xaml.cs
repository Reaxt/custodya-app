using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;
using System.Reflection;
using Microsoft.Azure.Devices;
using Firebase.Auth;

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
        HumidityChart.Series = ChartRepo<PlantsModel>.GetSeries(DataRepoProvider.PlantsDatabase.Items, "Humidity");
        TemperatureChart.Series = ChartRepo<PlantsModel>.GetSeries(DataRepoProvider.PlantsDatabase.Items, "Temperature");
        WaterChart.Series = ChartRepo<PlantsModel>.GetSeries(DataRepoProvider.PlantsDatabase.Items, "Water");
        MoistureChart.Series = ChartRepo<PlantsModel>.GetSeries(DataRepoProvider.PlantsDatabase.Items, "Moisture");

    }

    protected override async void OnAppearing()
    {
        Actuators.ItemsSource = App.DeviceTwinService.PlantActuators;
        await App.DeviceTwinService.UpdateActuators();
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
    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{Shell.Current.CurrentItem.Route}/Account", true);
    }
    

}