using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;

namespace Custodya;

public partial class PlantsPage : ContentPage
{
    public PlantsPage()
    {
        InitializeComponent();
        this.BindingContext = DataRepoProvider.PlantsDatabase;

    }
}