using Custodya.Interfaces;
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

        public string Name { get; set; }
        public bool State { get; set; } = false;
    }
}
