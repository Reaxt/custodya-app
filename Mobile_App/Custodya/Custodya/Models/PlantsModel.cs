using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Models
{
    /// <summary>
    /// The relevant model for plant data
    /// </summary>
    [Serializable]
    public class PlantsModel
    {
        /// <summary>
        /// The humidity sensor value
        /// </summary>
        public float Humidity { get; set; }
        /// <summary>
        /// The moisture sensor value
        /// </summary>
        public float Moisture { get; set; }
        /// <summary>
        /// The temperature sensor value
        /// </summary>
        public float Temperature { get; set; }
        /// <summary>
        /// The water sensor value
        /// </summary>
        public float Water { get; set; }
        /// <summary>
        /// The fan state (true = on. false = off)
        /// </summary>
        public bool Fan { get; set; }
        /// <summary>
        /// The led state (true = on. false = off)
        /// </summary>
        public bool Led { get; set; }

        public PlantsModel(float humidity, float moisture, float temperature, float water, bool fan, bool led) 
        {
            Humidity= humidity;
            Moisture= moisture;
            Temperature= temperature;
            Water= water;
            Fan= fan;
            Led= led;
        }
    }
}
