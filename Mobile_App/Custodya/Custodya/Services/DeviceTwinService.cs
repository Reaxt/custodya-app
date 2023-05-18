using Custodya.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Services
{
    /// <summary>
    /// This class is responsible for both reading and writing anything relevant to the device twin. 
    /// </summary>
    public class DeviceTwinService : IObserver<string>
    {
        EventHubService _eventHubService;
        private const string ACTUATOR_CONTROL_KEY = "actuatorControl";
        private const string UPDATE_KEY = "twinReportUpdate";

        private static string[] _securityActuatorsNames = new[] { "DoorLock" };
        private static string[] _plantActuatorsNames = new[] { "Led", "Fan" };
        private static string[] _geoActuatorsNames = new[] { "Buzzer" };
        /// <summary>
        /// A list of the security actuators.
        /// </summary>
        public ObservableCollection<Actuator> SecurityActuators { get; private set; }
        /// <summary>
        /// A list of the plant actuators.
        /// </summary>
        public ObservableCollection<Actuator> PlantActuators { get; private set; }
        /// <summary>
        /// A list of the geolocation actuators.
        /// </summary>
        public ObservableCollection<Actuator> GeoActuators { get; private set; }
        private RegistryManager _registryManager;
        private SemaphoreSlim _semaphore_update = new SemaphoreSlim(1, 1);
        public DeviceTwinService(EventHubService eventHubServier)
        {
            _registryManager = RegistryManager.CreateFromConnectionString(App.Settings.EventHubConnectionString);
            SecurityActuators = new ObservableCollection<Actuator>();
            PlantActuators = new ObservableCollection<Actuator>();
            GeoActuators = new ObservableCollection<Actuator>();
            _eventHubService = eventHubServier;
            _eventHubService.Subscribe(this);
        }
        /// <summary>
        /// Update all the actuators based off the reported device twin.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        public async Task UpdateActuators()
        {
            List<Actuator> actuators = await GetActuators();
            foreach(var actuator in actuators) 
            {
                AddOrUpdateCollection(_geoActuatorsNames, actuator, GeoActuators);
                AddOrUpdateCollection(_plantActuatorsNames, actuator, PlantActuators);
                AddOrUpdateCollection(_securityActuatorsNames, actuator, SecurityActuators);
            }
        }
        /// <summary>
        /// Add or update an actuator to a collection based on a filter.
        /// </summary>
        /// <param name="actuators">A list of strings to act as a inclusive filter (ignore if not in list)</param>
        /// <param name="actuator">The actuator to add or update</param>
        /// <param name="list">The list we are adding to or updating</param>
        private void AddOrUpdateCollection(string[] actuators, Actuator actuator, ObservableCollection<Actuator> list)
        {
            if(actuators.Contains(actuator.Name))
            {
                if(list.Any(x => x.Name == actuator.Name))
                {
                    int index = list.IndexOf(list.First(x => x.Name == actuator.Name));
                    list[index].PropertyChanged -= Actuator_PropertyChanged;
                    list[index].CopyValues(actuator);
                    list[index].PropertyChanged += Actuator_PropertyChanged;
                } else
                {
                    list.Add(actuator);
                    actuator.PropertyChanged += Actuator_PropertyChanged;
                }
            }
        }

        private async void Actuator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Actuator test = sender as Actuator;
            await ApplyChanges(test);
        }
        /// <summary>
        /// Gets all the actuators from the reported device twin.
        /// </summary>
        /// <returns>A list of actuators as described in the device twin.</returns>
        /// <exception cref="Exception">Throws if missing ACTUATOR_CONTROL_KEY</exception>
        private async Task<List<Actuator>> GetActuators()
        {
            Twin twin = await _registryManager.GetTwinAsync(App.Settings.DeviceId);
            string reported = twin.Properties.Reported.ToJson();
            
            
            JObject jsonTwin = JObject.Parse(reported);
            if(jsonTwin.ContainsKey(ACTUATOR_CONTROL_KEY))
            {
                List<Actuator> parsedActuators = new List<Actuator>();
                foreach (JToken token in jsonTwin[ACTUATOR_CONTROL_KEY])
                {
                    Actuator actuator = token.First.ToObject<Actuator>();
                    actuator.Name = ((JProperty)token).Name;
                    parsedActuators.Add(actuator);
                }

                return parsedActuators;
            } else
            {
                //no key!
                throw new Exception($"Device twin is missing key {ACTUATOR_CONTROL_KEY}");
            }
        }
        /// <summary>
        /// Apply actuator changes to the device twin.
        /// </summary>
        /// <param name="actuator">The actuator information to update the device twin with.</param>
        /// <returns>An awaitable task.</returns>
        public async Task ApplyChanges(Actuator actuator)
        {
            //one at a time!!! We need to use this as we could easily end up with the wrong ETag without it.
            await _semaphore_update.WaitAsync();
            var patch =
            $@"{{
                    properties: {{
                        desired: {{
                            {ACTUATOR_CONTROL_KEY}: {{
                                {actuator.Name}:
                                    {JsonConvert.SerializeObject(actuator)}
                            
                        }}
                    }}
                }}
            }}
            ";
            Twin twin = await _registryManager.GetTwinAsync(App.Settings.DeviceId);
            await _registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);
            _semaphore_update.Release();
        }

        public void OnCompleted()
        {
            //not implemented by event hub!
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            //not implemented by event hub!
            throw new NotImplementedException();
        }

        public async void OnNext(string value)
        {
            JObject message = JObject.Parse(value);
            //is it for us?
            JToken parsed = message[UPDATE_KEY];
            if (parsed != null)
            {
                dynamic test = parsed.ToObject<bool>();
                if(test)
                {
                    await UpdateActuators();
                }
            }
        }
    }
}
