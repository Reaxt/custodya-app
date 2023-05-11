using Custodya.Services;
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using Custodya.Config;

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
#if DEBUG
            await DisplayAlert("Config",
            $"{nameof(App.Settings.FireBaseAPIKey)}: {App.Settings.FireBaseAPIKey}\n" +
            $"{nameof(App.Settings.FireBase_DB_BaseUrl)}: {App.Settings.FireBase_DB_BaseUrl}\n" +
            $"{nameof(App.Settings.RefreshRate)}: {App.Settings.RefreshRate}\n" +
            $"{nameof(App.Settings.IsEnabled)}: {App.Settings.IsEnabled}", "OK");
#endif
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                // Connection to internet is available
                AuthService.UserCreds = await AuthService.Client.SignInWithEmailAndPasswordAsync(entryUsername.Text, entryPassword.Text);
                await DisplayAlert("Login", "Logged in successfully", "Ok");
                //uid.Text = $"Id: {AuthService.UserCreds.User.Uid}";
                //email.Text = $"Email: {entryUsername.Text}";
                //Login.IsVisible = false;
                //Logout.IsVisible = true;
                await Shell.Current.GoToAsync("//MauiFitness");
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

