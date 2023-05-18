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

	public MainPage()
	{
		InitializeComponent();
	}
    /// <summary>
    /// The log in button checks for the user credentials and then routs them to the proper view, 
    /// this even also controls if the user has connection to the internet or and and properly handles errors with descriptive messages
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        var popup = new LoadingPopup();
        this.ShowPopup(popup);
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
                popup.Close();
            }
            else
            {
                popup.Close();
                await DisplayAlert("Error", $"No internet connection. Can not authenticate.", "Ok");
            }
        }
        catch (FirebaseAuthException ex)
        {
            popup.Close();
            await DisplayAlert("Alert", $"Exception occured during Firebase Http request\nUrl: {ex.HelpLink}\nRequest Data:{ex.Source}\nResponse:{ex.Message}\nReason:{ex.Reason}", "Ok");
        }
        catch (Exception ex)
        {
            popup.Close();
            await DisplayAlert("Error", $"Exception occured: {ex.Message} from {ex.Source}", "Ok");
        }

    }
}

