using Custodya.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Repos
{
    /// <summary>
    /// This acts as a generic repo that can be used for any read-only data.
    /// </summary>
    /// <typeparam name="T">The type/model for the repository</typeparam>
    public class GenericSampleDataRepo<T> : IDeviceRepository<T>
    {
        private List<T> _sampleData;
        public GenericSampleDataRepo(List<T> sampleData) 
        {
            _sampleData= sampleData;
        }
        public async Task<T> GetLatestData()
        {
            return _sampleData.Last(); 
        }

        public async Task<IEnumerable<T>> GetPastData(int points = 10)
        {
            points = points > _sampleData.Count() ? points : _sampleData.Count();
            return _sampleData.GetRange(_sampleData.Count() - points, points);
        }
    }
}
