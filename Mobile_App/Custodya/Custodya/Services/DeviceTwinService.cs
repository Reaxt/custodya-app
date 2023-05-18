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
    public class DeviceTwinService
    {
        private const string ACTUATOR_CONTROL_KEY = "actuatorControl";

        private static string[] _securityActuatorsNames = new[] { "DoorLock" };
        private static string[] _plantActuatorsNames = new[] { "Led", "Fan" };
        private static string[] _geoActuatorsNames = new[] { "Buzzer" };

        public ObservableCollection<Actuator> SecurityActuators;
        public ObservableCollection<Actuator> PlantActuators;
        public ObservableCollection<Actuator> GeoActuators;
        private RegistryManager _registryManager;
        private SemaphoreSlim _semaphore_update = new SemaphoreSlim(1, 1);
        public DeviceTwinService()
        {
            _registryManager = RegistryManager.CreateFromConnectionString(App.Settings.EventHubConnectionString);
            SecurityActuators = new ObservableCollection<Actuator>();
            PlantActuators = new ObservableCollection<Actuator>();
            GeoActuators= new ObservableCollection<Actuator>();
        }

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
    }
}
