using Custodya.Interfaces;
using Custodya.Models;
using Custodya.Services;

namespace Custodya;

public partial class PodPage : ContentPage
{
    IGenericRealtimeDatabase<SecurityModel> _security;
	public PodPage()
	{
		_security = Handler.MauiContext.Services.GetService<IGenericRealtimeDatabase<SecurityModel>>();
		this.BindingContext = _security; //placeholder to test this out!
		InitializeComponent();
	}
}