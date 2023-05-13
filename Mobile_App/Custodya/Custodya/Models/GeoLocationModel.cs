using Custodya.Attributes;
using Custodya.Interfaces;

namespace Custodya.Models
{

    [Serializable, ModelJsonName("GeoLocation")]
    public class GeoLocationModel : ISubsystemState, IHasUKey
    {
        public Coordinate Coordinates { get; set; }
        public string CoordinatesString { 
            get
            {
                return Coordinates.Longitude.ToString() + " / " + Coordinates.Latitude.ToString();
            }
        }


        /// <summary>
        /// The current roll and pitch
        /// </summary>

        public Direction Heading { get; set; }

        public string HeadingString
        {
            get
            {
                return Heading.Pitch.ToString() + " / " + Heading.Roll.ToString();
            }
        }

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
            Coordinates = new() { Longitude = longitude, Latitude = latitude};
            Heading = new() { Pitch = pitch, Roll = roll };
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
    public struct Direction
    {
        public double Pitch;
        public double Roll;
    }
}
