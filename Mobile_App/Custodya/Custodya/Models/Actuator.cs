using Custodya.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Models
{
    public class Actuator
    {
        public static string[] SecurityActuators = new[] { "DoorLock" };
        public static string[] PlantActuators = new[] { "Led", "Fan" };
        public static string[] GeoActuators = new[] { "Buzzer" };
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ControlMethods
        {
            rules,
            manual
        }
        [JsonIgnore]
        public string Name { get; set; }
        [JsonProperty("manualState")]
        public bool State { get; set; } = false;
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("controlMethod")]
        public ControlMethods ControlMethod { get; set; }
        [JsonProperty("rules")]
        public List<ActuatorRule> Rules { get; set; }
    }
}
