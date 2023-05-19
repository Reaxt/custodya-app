using Custodya.Models;
using Custodya.Repos;
using System.ComponentModel;
using System.Web;

namespace Custodya;
public partial class ChartDetailPage : ContentPage, IQueryAttributable
{
    ChartRepo<PlantsModel> plantsChart;
    ChartRepo<SecurityModel> securityChart;
    public ChartRepo<PlantsModel> PlantsChart 
    { 
        get => plantsChart; 
        set {
            object? o = value;
            plantsChart = value; 
        } 
    }
    public ChartRepo<SecurityModel> SecurityChart 
    { 
        get => securityChart; 
        set {
            object? o = value;
            securityChart = value; 
        } 
    }
    public string BackPage { get; set; }
    public ChartDetailPage()
    {
        InitializeComponent();        
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            string plants = query["PlantsChart"].ToString();
            string security = query["SecurityChart"].ToString();
            PlantsChart = plants == string.Empty ? null : new ChartRepo<PlantsModel>(DataRepoProvider.PlantsDatabase.Items, plants, 100);
            SecurityChart = security == string.Empty ? null : new ChartRepo<SecurityModel>(DataRepoProvider.SecurityDatabase.Items, security, 100);
            BackPage = query["back"].ToString();
            if (PlantsChart == null)
            {
                Title.Text = $"Chart: {plants}";
                BigChart.Series = SecurityChart.DataSeries;
                BigChart.XAxes = ChartRepo<SecurityModel>.XAxis;
                BigChart.YAxes = ChartRepo<SecurityModel>.YAxis;
            }
            else
            {
                Title.Text = $"Chart: {plants}";
                BigChart.Series = PlantsChart.DataSeries;
                BigChart.XAxes = ChartRepo<PlantsModel>.XAxis;
                BigChart.YAxes = ChartRepo<PlantsModel>.YAxis;
            } 
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{BackPage}");
    }
}