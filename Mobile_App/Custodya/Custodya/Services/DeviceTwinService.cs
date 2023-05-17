using Custodya.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Services
{
    public class DeviceTwinService
    {
        private const string ACTUATOR_CONTROL_KEY = "actuatorControl";

        private static string[] SecurityActuators = new[] { "DoorLock" };
        private static string[] PlantActuators = new[] { "Led", "Fan" };
        private static string[] GeoActuators = new[] { "Buzzer" };
        private RegistryManager _registryManager;
        public DeviceTwinService()
        {
            _registryManager = RegistryManager.CreateFromConnectionString(App.Settings.EventHubConnectionString);

        }
        public async Task<List<Actuator>> GetActuators(IEnumerable<string> actuators)
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
                    if(actuators.Contains(actuator.Name)) parsedActuators.Add(actuator);
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
        }
    }
}
