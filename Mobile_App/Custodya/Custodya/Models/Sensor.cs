using Custodya.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Models
{

    public class Sensor: IHasUKey, ISubsystemState
    {
        public static string[] SecuritySensors = new[] { "Motion", "Loudness", "Door" };
        public static string[] SecurityActuators = new[] { "DoorLock" };
        public enum SensorState { Valid, Error }

        public string Name { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public dynamic Value { get; set; }
        public bool Editable { get; set; } = true;
        public string Key { get; set; }
        public SensorState State { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
