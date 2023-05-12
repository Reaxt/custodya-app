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
        public Coordinate Coordinates { get; set; }


        /// <summary>
        /// The current roll and pitch
        /// </summary>

        public Heading Heading { get; set; }



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
            Coordinates.Longitude.Equals(longitude);
            Coordinates.Latitude.Equals(latitude);
            Heading.Pitch.Equals(pitch);
            Heading.Roll.Equals(roll);
            Buzzer = buzzer;
            InTransport = inTransport;
        }
    }

    [Serializable]
    public struct Coordinate
    {
        public double Longitude;
        public double Latitude;
    }

    [Serializable]
    public struct Heading
    {
        public double Pitch;
        public double Roll;
    }
}
