using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModelHasSensors : Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RuleCompatibleProperty : Attribute
    {
        public enum EntryOption
        {
            Float,
            Bool,
            Options,
            Enum
        }
        public EntryOption Option;
        public string[] Options { get; set; }
        public RuleCompatibleProperty(EntryOption option)
        {
            Option = option;
        }
    }
}
