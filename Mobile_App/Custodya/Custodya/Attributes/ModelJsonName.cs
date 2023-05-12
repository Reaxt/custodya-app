using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ModelJsonName : Attribute
    {
        public string Name { get; set; }
        public ModelJsonName(string name) { this.Name = name; }
    }
}
