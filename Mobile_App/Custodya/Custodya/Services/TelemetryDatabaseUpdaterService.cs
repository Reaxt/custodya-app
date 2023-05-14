using Custodya.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Services
{
    /// <summary>
    /// A service to update a database with new telemetry received from the Azure IOT Event Hub
    /// </summary>
    /// <typeparam name="T">The model we are updating</typeparam>
    /// <remarks>Make sure you have an IGenericDatabase<<typeparamref name="T"/>> and TelemtryJsonParser<<typeparamref name="T"/>> subscribed already</remarks>
    public class TelemetryDatabaseUpdaterService<T> : IObserver<T> where T : class, ISubsystemState
    {
        private IGenericDatabase<T> _database;
        TelemetryJsonParser<T> _telemetry;
        public TelemetryDatabaseUpdaterService(IGenericDatabase<T> database) 
        {
            this._database = database;
            this._telemetry = new TelemetryJsonParser<T>(MauiProgram.Services.GetService<EventHubService>());
            this._telemetry.Subscribe(this);
        }

        public void OnCompleted()
        {
            //this isnt implemented by TelemtryJsonParser and should never be thrown!
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            //this isnt implemented by TelemtryJsonParser and should never be thrown!
            throw new NotImplementedException();
        }

        public async void OnNext(T value)
        {
            if(!await _database.CheckExists(value))
            {
                await _database.AddItemAsync(value);
            }
        }
    }
}
