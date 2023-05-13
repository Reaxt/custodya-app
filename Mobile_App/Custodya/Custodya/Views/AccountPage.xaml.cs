using Custodya.Services;
using Firebase.Auth;

namespace Custodya;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
        this.BindingContext = AuthService.UserCreds.User.Info;        
	}

	private async void btnLogout_Clicked(object sender, EventArgs e)
	{
        try
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                // Connection to internet is available
                AuthService.Client.SignOut();
                await Shell.Current.GoToAsync("//Login");
            }
            else
            {
                throw new Exception("No internet connection");
            }
        }
        catch (FirebaseAuthException ex)
        {
            await DisplayAlert("Alert", $"Exception occured during Firebase Http request\nUrl: {ex.HelpLink}\nRequest Data:{ex.Source}\nResponse:{ex.Message}\nReason:{ex.Reason}", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", $"{ex.Message}", "Ok");
        }
    }
}