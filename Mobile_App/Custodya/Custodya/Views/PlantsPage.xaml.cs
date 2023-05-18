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
        _waterChartRepo    = new ChartRepo<PlantsModel>(DataRepoProvider.PlantsDatabase.Items, "Water", 20);
        _temperatureChartRepo = new ChartRepo<PlantsModel>(DataRepoProvider.PlantsDatabase.Items, "Temperature", 20);
        InitializeComponent();
        
        this.BindingContext = DataRepoProvider.PlantsDatabase;
        HumidityChart.Series = _humidityChartRepo.DataSeries;
        TemperatureChart.Series = _temperatureChartRepo.DataSeries;
        WaterChart.Series = _waterChartRepo.DataSeries;
        MoistureChart.Series = _moistureChartRepo.DataSeries;
        
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