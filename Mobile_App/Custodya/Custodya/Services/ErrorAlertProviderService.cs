using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Services
{
    public class ErrorAlertProviderService
    {
        public event EventHandler<ErrorAlertEventArgs> ErrorEvent;
        private IConnectivity connectivity;
        public ErrorAlertProviderService() 
        {
            connectivity = Connectivity.Current;
            connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                RaiseError("Lost internet connection!", ErrorAlertEventArgs.ErrorLogType.Error);
            }
        }
        public void RaiseError(string error, ErrorAlertEventArgs.ErrorLogType errorLogType = ErrorAlertEventArgs.ErrorLogType.Error)
        {
            //cancel if its a debug log and we are in release mode!
            if(errorLogType == ErrorAlertEventArgs.ErrorLogType.Debug)
            {
#if (!DEBUG)
                return;
#endif
            }
            EventHandler<ErrorAlertEventArgs> errorEvent = ErrorEvent;
            if(errorEvent != null)
            {
                var errorAlertEventArgs = new ErrorAlertEventArgs(error, errorLogType);
                errorEvent(this, errorAlertEventArgs);
            }
        }

    }
    public class ErrorAlertEventArgs : EventArgs
    {
        public enum ErrorLogType
        {
            Debug,
            Warning,
            Information,
            Error
        }
        public string Message { get; set; }
        public ErrorLogType LogType { get; set; }
        public ErrorAlertEventArgs(string message, ErrorLogType logType)
        {
            this.LogType = logType;
            this.Message = message;
        }
    }
}
