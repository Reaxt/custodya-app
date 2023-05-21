using Custodya.Services;
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using Custodya.Config;
using Custodya.Repos;
using Custodya.ControlTemplates;
using CommunityToolkit.Maui.Views;

namespace Custodya;

public partial class MainPage : ContentPage
{
    private const string CACHE_EMAIL = "UserEmail";
    private const string CACHE_PASSWORD = "UserPassword";
    private string _cachedEmail;
    private string _cachedPassword;
	public MainPage()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var cached = AuthService.GetCachedCredentials();
        entryPassword.Text = cached.password;
        entryUsername.Text = cached.username;
        checkRememberMe.IsChecked = true;
        //TODO: make auto login work
        //await PerformLogin(cached.username, cached.password, true, false);
    }
    /// <summary>
    /// The log in button checks for the user credentials and then routs them to the proper view, 
    /// this even also controls if the user has connection to the internet or and and properly handles errors with descriptive messages
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        PerformLogin(entryUsername.Text, entryPassword.Text, checkRememberMe.IsChecked);
    }
    private async Task PerformLogin(string username, string password, bool writePrefs, bool doPopup = true)
    {
        var popup = new LoadingPopup();
        if(doPopup)
        {
            this.ShowPopup(popup);
        }
        try
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                // Connection to internet is available
                AuthService.UserCreds = await AuthService.Client.SignInWithEmailAndPasswordAsync(username, password);
                if(writePrefs)
                {
                    AuthService.SetCachedCredentials(username, password);
                } else
                {
                    AuthService.ResetCachedCredentials();
                }
                DataRepoProvider.InitDb();
                string destination = entryUsername.Text.Split('@')[0].ToLower() == "owner" ? "Owner" : "User";
                await Shell.Current.GoToAsync($"//{destination}");
                if (doPopup)
                {
                    popup.Close();
                }
            }
            else
            {
                if (doPopup)
                {
                    popup.Close();
                }
                await DisplayAlert("Error", $"No internet connection. Can not authenticate.", "Ok");
            }
        }
        catch (FirebaseAuthException ex)
        {
            if(doPopup)
            {
                popup.Close();
            }
            AuthService.ResetCachedCredentials();
            await DisplayAlert("Error", $"Failed to log in: {ex.Message}", "Ok");

        }
        catch (Exception ex)
        {
            if (doPopup)
            {
                popup.Close();
            }
            AuthService.ResetCachedCredentials();
            await DisplayAlert("Error", $"Exception occured: {ex.Message} from {ex.Source}", "Ok");
        }
    }
}

