using Custodya.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Custodya.Attributes;
using System.Reactive.Linq;
using System.Reactive;
using System.Threading;

namespace Custodya.Services
{
    public class TelemetryJsonParser<T> : IObservable<T>, IObserver<string> where T : class, ISubsystemState
    {
        private EventHubService _hubService;
        private string _jsonModelKey;
        private List<T> _previousVals;
        private List<IObserver<T>> _observers;
        private SemaphoreSlim _jsonParserSemaphore;
        public TelemetryJsonParser(EventHubService hubService) 
        {
            var attr = typeof(T).GetCustomAttribute<ModelJsonName>();
            if (attr == null)
            {
                //Default to the class name..
                _jsonModelKey = typeof(T).Name;
            } else
            {
                _jsonModelKey = attr.Name;
            }
            _jsonParserSemaphore = new SemaphoreSlim(1, 1);
            _previousVals = new List<T>();
            _observers = new List<IObserver<T>>();
            _hubService = hubService;
            _hubService.Subscribe(this);
        }
        public void OnCompleted()
        {
            //This isnt implemented by _hubservice, and should never be thrown!
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            //this isnt implemented by _hubService, and should never be thrown!
            throw new NotImplementedException();
        }

        public async void OnNext(string value)
        {
            JObject telemetry = JObject.Parse(value);
            //is it for us?
            JToken modelJson = telemetry[_jsonModelKey];
            if(modelJson != null)
            {
                T model = null;
                model = modelJson.ToObject<T>();
                long posix = (long)telemetry["timestamp"];
                var timeOffset = DateTimeOffset.FromUnixTimeSeconds(posix);
                model.Timestamp = timeOffset.LocalDateTime;
                SendOutModel(model);
            }    
        }
        private async void SendOutModel(T model)
        {
            await _jsonParserSemaphore.WaitAsync();
            _previousVals.Add(model);
            foreach (var observer in _observers)
            {
                observer.OnNext(model);
            }
            _jsonParserSemaphore.Release();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if(!_observers.Contains(observer))
            {
                _observers.Add(observer);
                //send out all we missed
                HandoutPreviousVals(observer);
            }
            return new Unsubscriber<T>(_observers, observer);
        }
        public async void HandoutPreviousVals(IObserver<T> observer)
        {
            await _jsonParserSemaphore.WaitAsync();
            foreach (var item in _previousVals)
            {
                observer.OnNext(item);
            }
            _jsonParserSemaphore.Release();
        }
    }
}
