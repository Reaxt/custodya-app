using Custodya.Models;
using Custodya.Repos;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Custodya;

/// <summary>
/// bindingContext for the GeoLocation DataBase
/// </summary>
public partial class GeoPage : ContentPage
{
    public GeoPage()
    {
        InitializeComponent();
        this.BindingContext = DataRepoProvider.GeolocationDatabase;
    }
}