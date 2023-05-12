 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Custodya.Attributes;
using Custodya.Interfaces;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace Custodya.Models
{

    [Serializable, ModelJsonName("GeoLocation")]
    public class GeoLocationModel : ISubsystemState, IHasUKey
    {
        /// <summary>
        /// The current Longitude
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// The current latitude
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// The current pitch
        /// </summary>
        public double Pitch { get; set; }
        /// <summary>
        /// The current roll
        /// </summary>
        public double Roll { get; set; }
        /// <summary>
        /// The buzzer state (on = true, off = false)
        /// </summary>
        public bool Buzzer { get; set; }
        /// <summary>
        /// InTransport 
        /// </summary>
        public bool InTransport { get; set; }

        public string JsonKey { get; set; }
        public DateTime Timestamp { get; set; }
        public string Key { get; set; }

        public GeoLocationModel(double longitude, double latitude, double pitch, double roll, bool buzzer, bool inTransport)
        {
            Longitude = longitude;
            Latitude = latitude;
            Pitch = pitch;
            Roll = roll;
            Buzzer = buzzer;
            InTransport = inTransport;
        }
    }


}
