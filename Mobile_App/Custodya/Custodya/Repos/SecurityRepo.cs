using Custodya.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Repos
{
    class SecurityRepo
    {
        private ObservableCollection<SecurityModel> securities = new ObservableCollection<SecurityModel>();

        public ObservableCollection<SecurityModel> Securities
        {
            get { return securities; }

        }

        public void AddData()
        {
            securities.Add(new SecurityModel(true, 30, SecurityModel.DoorState.Open, SecurityModel.DoorState.Open));
            securities.Add(new SecurityModel(false, 5, SecurityModel.DoorState.Closed, SecurityModel.DoorState.Closed));
        }
    }
}
