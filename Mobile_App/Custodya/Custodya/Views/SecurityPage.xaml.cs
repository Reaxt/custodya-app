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
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Custodya;

public partial class SecurityPage : ContentPage
{
    /// <summary>
    /// binds the XAML to the database data, and shows the right user the information that they are intended to see
    /// </summary>
    private ChartRepo<SecurityModel> _loudnessChart;

    public SecurityPage()
	{
        _loudnessChart = new ChartRepo<SecurityModel>(DataRepoProvider.SecurityDatabase.Items, "Loudness", 20);
        InitializeComponent();

        this.BindingContext = DataRepoProvider.SecurityDatabase;
        if (Shell.Current.CurrentItem.Route == "Owner")
        {
            controlFrame.IsVisible = false;
        }
        Chart.Series = _loudnessChart.DataSeries;
        Chart.XAxes = ChartRepo<SecurityModel>.XAxis;
        Chart.YAxes = ChartRepo<SecurityModel>.YAxis;
    }
    
    protected override async void OnAppearing()
    {
        Actuators.ItemsSource = App.DeviceTwinService.SecurityActuators;
        await App.DeviceTwinService.UpdateActuators();
    }

    private async void ibtnAccount_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{Shell.Current.CurrentItem.Route}Account");
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

    private async void btnChart_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//ChartView?SecurityChart=Loudness&PlantsChart={null}&back={Shell.Current.CurrentItem.Route}");
    }
}