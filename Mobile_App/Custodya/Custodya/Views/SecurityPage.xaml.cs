using Custodya.Models;
using System.Collections.ObjectModel;
using Custodya.Repos;

namespace Custodya;

public partial class SecurityPage : ContentPage
{
    public SecurityPage()
	{
        InitializeComponent();
        this.BindingContext = DataRepoProvider.SecurityDatabase;
        if (Shell.Current.CurrentItem.Route == "Owner")
        {
            controlFrame.IsVisible = false;
        }
    }
}