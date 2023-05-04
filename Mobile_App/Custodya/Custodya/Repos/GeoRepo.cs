using Custodya.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Repos
{
    class GeoRepo<T>
    {
        private ObservableCollection<GeoLocationModel> geoLocations = new ObservableCollection<GeoLocationModel>();

        public ObservableCollection<GeoLocationModel> GeoLocations 
        { 
            get { return geoLocations; }
                
        } 

        public void AddData()
        {
            geoLocations.Add(new GeoLocationModel(45.49599318218543,-73.78425572261361,66,88,true,true));
            geoLocations.Add(new GeoLocationModel(45.49874044085865, -73.74439569799661, 0, 0, false, false));
        }
    }
}
