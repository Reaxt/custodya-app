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
    }
}