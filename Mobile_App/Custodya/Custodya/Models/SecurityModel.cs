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
    [Serializable]
    public class SecurityModel
    {
        [Serializable]
        public enum DoorState
        {
            Open,
            Closed
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
        public SecurityModel(bool motion, float loudness, DoorState doorstate, DoorState lockstate)
        {
            Motion = motion;
            Loudness = loudness;
            Door = doorstate;
            DoorLock = lockstate;
        }
    }
}
