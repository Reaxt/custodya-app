using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Models
{
    public class ActuatorRule
    {
        public string TargetReadingType { get; set; }
        public dynamic targetValue { get; set; }
        public string ComparisonType { get; set; }
        public bool valueOnRule { get; set; }
    }
}
