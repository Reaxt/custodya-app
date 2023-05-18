using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodya.Models
{
    public class ActuatorRule
    {
        [JsonIgnore]
        private static readonly string[] _comparisonTypes = { ">", "<", "==" };
        [JsonProperty("targetReadingType")]
        public string TargetReadingType { get; set; }
        [JsonProperty("targetValue")]
        public dynamic TargetValue { get; set; }
        private string _comparisonType;
        [JsonProperty("comparisonType")]
        public string ComparisonType { get { return _comparisonType; } set 
            {
                //validation here as we can't easily make an enum for the comparison types.   
                if(_comparisonTypes.Contains(value))
                {
                    _comparisonType = value;
                } else
                {
                    throw new InvalidDataException("This is not a valid comparison type!");
                }
            }
        }
        [JsonProperty("valueOnRule")]
        public bool ValueOnRule { get; set; }
    }
}
