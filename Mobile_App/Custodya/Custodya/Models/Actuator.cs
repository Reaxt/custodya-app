using Custodya.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Models
{
    public class Actuator: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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


        public void CopyValues(Actuator obj)
        {
            this.Name = obj.Name;
            this.State = obj.State;
            this.Rules = obj.Rules;
            this.ControlMethod= obj.ControlMethod;
        }
    }
}
