using Custodya.Services;
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using Custodya.Config;
using Custodya.Repos;

namespace Custodya;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        try
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                // Connection to internet is available
                AuthService.UserCreds = await AuthService.Client.SignInWithEmailAndPasswordAsync(entryUsername.Text, entryPassword.Text);
                DataRepoProvider.InitDb();
                string destination = entryUsername.Text.Split('@')[0].ToLower() == "owner" ? "Owner" : "User";
                await Shell.Current.GoToAsync($"//{destination}");
                //uid.Text = $"Id: {AuthService.UserCreds.User.Uid}";
                //email.Text = $"Email: {entryUsername.Text}";
                //Login.IsVisible = false;
                //Logout.IsVisible = true;
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

