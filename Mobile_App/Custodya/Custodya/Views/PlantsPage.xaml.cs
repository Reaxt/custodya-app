using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;

namespace Custodya;

public partial class PlantsPage : ContentPage
{
    /// <summary>
    /// binds the plant database to its corespiting XMAL part
    /// </summary>
    public PlantsPage()
    {
        InitializeComponent();
        this.BindingContext = DataRepoProvider.PlantsDatabase;

    }
}