using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;
using Custodya.Interfaces;

namespace Custodya;

public partial class PlantsPage : ContentPage
{
    IGenericRealtimeDatabase<PlantsModel> _database;
    public PlantsPage(IGenericRealtimeDatabase<PlantsModel> plantsDatabase)
	{
        
		InitializeComponent();
    }
}