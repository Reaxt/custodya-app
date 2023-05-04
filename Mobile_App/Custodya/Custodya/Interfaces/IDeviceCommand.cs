using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Interfaces
{
    public interface IDeviceCommand<T>
    {
        string MethodName { get; }
        T Payload { get; }

    }
}
