using Custodya.Attributes;
using Custodya.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Models
{
    /// <summary>
    /// The model representing received security data
    /// </summary>
    [Serializable, ModelJsonName("Security")]
    public class SecurityModel : IHasUKey, ISubsystemState
    {
        
        [Serializable]
        public enum DoorState
        {
            Open = 0,
            Closed = 1
        }
        /// <summary>
        /// The motion sensor value
        /// </summary>
        public bool Motion { get; set; }
        /// <summary>
        /// Loudness sensor value
        /// </summary>
        public float Loudness { get; set; }
        /// <summary>
        /// Whether the door is currently open or closed
        /// </summary>
        public DoorState Door { get; set; } 
        /// <summary>
        /// Whether the door is currently locked or unlocked
        /// </summary>
        public DoorState DoorLock { get; set; }
        public DateTime Timestamp { get; set; }
        public string Key { get; set; }

        public SecurityModel(bool motion, float loudness, DoorState doorstate, DoorState lockstate)
        {
            Motion = motion;
            Loudness = loudness;
            Door = doorstate;
            DoorLock = lockstate;
        }
    }
}
