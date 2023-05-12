using Custodya.Interfaces;
using Custodya.Models;
using Custodya.Repos;
using Custodya.Services;

namespace Custodya;

public partial class PodPage : ContentPage
{
	public PodPage()
	{
		this.BindingContext = DataRepoProvider.SecurityDatabase;
		InitializeComponent();
	}
}