using Custodya.Services;
using Microsoft.Extensions.Configuration;
namespace Custodya;

public partial class AppShell : Shell
{
    private ErrorAlertProviderService _errorAlertProviderService;
    public AppShell()
	{
		InitializeComponent();
        _errorAlertProviderService = MauiProgram.Services.GetService<ErrorAlertProviderService>();
        _errorAlertProviderService.ErrorEvent += ErrorAlertProviderService_ErrorEvent;
	}

    private async void ErrorAlertProviderService_ErrorEvent(object sender, ErrorAlertEventArgs e)
    {
        string title = "Alert";
        switch (e.LogType)
        {
            case ErrorAlertEventArgs.ErrorLogType.Debug:
                title= "Debug";
                break;
            case ErrorAlertEventArgs.ErrorLogType.Warning:
                title = "Warning";
                break;
            case ErrorAlertEventArgs.ErrorLogType.Error:
                title= "Error";
                break;
            case ErrorAlertEventArgs.ErrorLogType.Information:
                title = "Info";
                break;
        }
        await DisplayAlert(title, e.Message, "Ok");
    }
}
