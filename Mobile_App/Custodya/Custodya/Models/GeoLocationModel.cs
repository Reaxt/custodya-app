 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace Custodya.Models
{

    [Serializable]
    public class GeoLocationModel
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
        public bool BuzzerState { get; set; }
        /// <summary>
        /// InTransport 
        /// </summary>
        public bool InTransport { get; set; }

        public GeoLocationModel(double longitude, double latitude, double pitch, double roll, bool buzzerState, bool inTransport)
        {
            Longitude = longitude;
            Latitude = latitude;
            Pitch = pitch;
            Roll = roll;
            BuzzerState = buzzerState;
            InTransport = inTransport;
        }
    }


}
