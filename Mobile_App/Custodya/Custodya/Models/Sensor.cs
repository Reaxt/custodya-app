using Custodya.Interfaces;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Models
{

    public class Sensor : INotifyPropertyChanged
    {
        public static string[] SecuritySensors = new[] { "Motion", "Loudness", "Door" };        
        public static string[] PlantSensors = new[] { "Temperature", "Moisture", "Humidity", "Water" };        
        public static string[] GeoSensors = new[] { "CoordinatesString", "HeadingString", "InTransport"};

        public event PropertyChangedEventHandler PropertyChanged;

        public enum SensorState { Valid, Error }

        public string Name { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public dynamic Value { get; set; }
        public bool Editable { get; set; } = true;
        public SensorState State { get; set; }
        public ObservableCollection<ISeries> Series { get; set; }
    }
}
