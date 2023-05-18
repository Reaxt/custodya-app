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
    [Serializable, ModelJsonName("Security"), ModelHasSensors]
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
        [RuleCompatibleProperty(RuleCompatibleProperty.EntryOption.Bool)]
        public bool Motion { get; set; }
        /// <summary>
        /// Loudness sensor value
        /// </summary>
        [RuleCompatibleProperty(RuleCompatibleProperty.EntryOption.Float)]
        public float Loudness { get; set; }
        /// <summary>
        /// Whether the door is currently open or closed
        /// </summary>
        [RuleCompatibleProperty(RuleCompatibleProperty.EntryOption.Bool, Options = new string[]{"Open", "Closed"})]
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
