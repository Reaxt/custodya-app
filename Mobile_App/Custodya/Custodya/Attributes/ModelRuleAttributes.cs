using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Attributes
{
    /// <summary>
    /// An attribute to define this model as having rule-compatible sensors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModelHasRuleSensors : Attribute
    {

    }
    /// <summary>
    /// An attribute to describe the rule-compatible property.
    /// </summary>
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
        /// <summary>
        /// The option for this property, used to handle input and serializing.
        /// </summary>
        public EntryOption Option;
        /// <summary>
        /// IF we are an Options entry, these are what we pick from.
        /// </summary>
        public string[] Options { get; set; }
        /// <summary>
        /// An attribute to describe the rule-compatible property.
        /// </summary>
        /// <param name="option">The EntryOption type for this property.</param>
        public RuleCompatibleProperty(EntryOption option)
        {
            Option = option;
        }
    }
}
