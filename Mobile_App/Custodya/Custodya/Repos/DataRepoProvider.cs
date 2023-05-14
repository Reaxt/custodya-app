using Custodya.Interfaces;
using Custodya.Models;
using Custodya.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Repos
{
    public static class DataRepoProvider
    {
        public static IGenericRealtimeDatabase<SecurityModel> SecurityDatabase { get; private set; } = null;
        public static IGenericRealtimeDatabase<GeoLocationModel> GeolocationDatabase { get; private set; } = null;
        public static IGenericRealtimeDatabase<PlantsModel> PlantsDatabase { get; private set; } = null;
        /// <summary>
        /// Once we're authenticated, use this to initialise the db
        /// </summary>
        public static void InitDb() 
        {
            if (SecurityDatabase== null) SecurityDatabase = new FirebaseRealtimeDatabaseService<SecurityModel>();
            if (GeolocationDatabase == null) GeolocationDatabase = new FirebaseRealtimeDatabaseService<GeoLocationModel>();
            if (PlantsDatabase == null) PlantsDatabase = new FirebaseRealtimeDatabaseService<PlantsModel>();
        }
    }
}
