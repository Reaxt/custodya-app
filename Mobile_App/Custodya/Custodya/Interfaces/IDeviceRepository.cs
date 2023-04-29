using Custodya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Interfaces
{
    public interface IDeviceRepository<T>
    {
        Task<T> GetLatestData();
        Task<IEnumerable<T>> GetPastData(int points = 10);

    }
}
