using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Config
{
    public class Settings
    {
        //Event Hub Connection Strings
        public string EventHubConnectionString { get; set; }

        public string EventHubName { get; set; }

        public string ConsumerGroup { get; set; }

        public string StorageConnectionString { get; set; }

        public string BlobContainerName { get; set; }


        //IoT Hub Connection Strings
       // public string HubConnectionString { get; set; }
       // public string DeviceId { get; set; }

        //Firebase Strings and URL
        public int RefreshRate { get; set; }
        public string FireBaseAPIKey { get; set; }
        public string FireBase_DB_BaseUrl { get; set; }
        public bool IsEnabled { get; set; }
        public string FireBase_Authorized_Domain { get; set; }


    }
}
