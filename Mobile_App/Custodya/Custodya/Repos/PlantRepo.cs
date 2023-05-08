using Custodya.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Repos
{
    class PlantRepo
    {
        private ObservableCollection<PlantsModel> plants = new ObservableCollection<PlantsModel>();

        public ObservableCollection<PlantsModel> Plants
        {
            get { return plants; }

        }

        public void AddData()
        {
            plants.Add(new PlantsModel((float)4.5,(float)33.71, 37, 10, true, true));
            plants.Add(new PlantsModel((float)10.25,(float)7.74, 0, 0, false, false));
        }
    }
}
