using Custodya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Repos
{
    /// <summary>
    /// This class is used to get the sample data repositories.
    /// </summary>
    public static class SampleData
    {
        private const int LISTSIZE = 20;
        //private static List<GeoLocationModel> geoLocationModels= new List<GeoLocationModel>() { };
        //private static List<SecurityModel> securityModels = new List<SecurityModel>() { };
        //private static List<PlantsModel> plantsModels= new List<PlantsModel>() { };

        private static GenericSampleDataRepo<GeoLocationModel> _geolocationRepo;
        private static GenericSampleDataRepo<SecurityModel> _securityRepo;
        private static GenericSampleDataRepo<PlantsModel> _plantsRepo;

        public static GenericSampleDataRepo<GeoLocationModel> GeolocationRepo 
        { 
            get 
            {
                if(_geolocationRepo == null)
                {
                    List<GeoLocationModel> geoLocationModels= new List<GeoLocationModel>();
                    for (int i = 0; i < LISTSIZE; i++)
                    {
                        geoLocationModels[i] = randomGeolocation();
                    }
                    _geolocationRepo = new GenericSampleDataRepo<GeoLocationModel>(geoLocationModels);
                }
                return _geolocationRepo;
            }
        }
        public static GenericSampleDataRepo<SecurityModel> SecurityRepo
        {
            get
            {
                if (_securityRepo == null)
                {
                    List<SecurityModel> securityModels = new List<SecurityModel>();
                    for (int i = 0; i < LISTSIZE; i++)
                    {
                        securityModels[i] = randomSecurity();
                    }
                    _securityRepo = new GenericSampleDataRepo<SecurityModel>(securityModels);
                }
                return _securityRepo;
            }
        }
        public static GenericSampleDataRepo<PlantsModel> PlantsRepo
        {
            get
            {
                if(_plantsRepo == null)
                {
                    List<PlantsModel> plantsModels = new List<PlantsModel>();
                    for (int i = 0; i < LISTSIZE; i++)
                    {
                        plantsModels[i] = randomPlant();
                    }
                    _plantsRepo = new GenericSampleDataRepo<PlantsModel>(plantsModels);
                }
                return _plantsRepo;
            }
        }

        private static GeoLocationModel randomGeolocation()
        {
            Random random = new Random();
            return new GeoLocationModel(
                random.NextDouble()*10,
                random.NextDouble()*10,
                random.NextDouble()*10,
                random.NextDouble() * 10,
                random.Next(0,2)==1,
                random.Next(0,2)==1
                );
        }
        private static PlantsModel randomPlant()
        {
            Random random = new Random();
            return new PlantsModel(
                (float)random.NextDouble() * 10,
                (float)random.NextDouble() * 10,
                (float)random.NextDouble() * 10,
                (float)random.NextDouble() * 10,
                random.Next(0, 2) == 1,
                random.Next(0, 2) == 1);
        }
        private static SecurityModel randomSecurity()
        {
            Random random = new Random();
            return new SecurityModel(
                random.Next(0, 2) == 1,
                (float)random.NextDouble() * 10,
                (SecurityModel.DoorState)random.Next(0, 2),
                (SecurityModel.DoorState)random.Next(0, 2));
        }
    }
}
