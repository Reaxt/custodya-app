using Custodya.Attributes;
using Custodya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.ViewModels
{
    public struct RuleType
    {
        public string TargetReading { get; set; }
        public string[] ComparisonTypes { get; set; }
        public RuleCompatibleProperty.EntryOption ComparisonValueType { get; set; }
        public Type PropertyType { get; set; }
        public string[] Options { get; set; }
        public dynamic Default { get; set; }
    }
    /// <summary>
    /// A container class for editing actuator rules.
    /// </summary>
    public class RuleContainer
    {
        public RuleType RuleType { get; set; }
        public dynamic TargetValue { get; set; }
        public string ComparisonType { get; set; }
        public bool ValueOnRule { get; set; }
        public float TargetValueFloat { get; set; } //This property exists for elegant binding of the float type.
        public bool TargetValueBool { get; set; } //This property exists for elegant binding of the bool type..
        public static List<RuleType> _ruleTypesCache = null; //reflection is expensive.. lets make sure we only ever do it once!
        /// <summary>
        /// Create a new RuleContainer based off an existing rule.
        /// </summary>
        /// <param name="ruleType">The ruleType for this rules parameter</param>
        /// <param name="rule">The existing rule</param>
        public RuleContainer(RuleType ruleType, ActuatorRule rule)
        {
            this.RuleType = ruleType;
            this.TargetValue = rule.TargetValue;
            this.ComparisonType = rule.ComparisonType;
            this.ValueOnRule= rule.ValueOnRule;
        }
        /// <summary>
        /// Create a new <c>RuleContainer</c> based off a RuleType
        /// </summary>
        /// <param name="ruleType">The RuleType for this rules parameter.</param>
        public RuleContainer(RuleType ruleType)
        {
            this.RuleType = ruleType;
            this.TargetValue = ruleType.Default;
            this.ComparisonType = ruleType.ComparisonTypes[0];
            this.ValueOnRule = true;
        }
        /// <summary>
        /// Obtain an ActuatorRule object from this container.
        /// </summary>
        /// <returns>The ActuatorRule this container represents.</returns>
        public ActuatorRule ToRule()
        {
            if (RuleType.ComparisonValueType == RuleCompatibleProperty.EntryOption.Float) TargetValue = TargetValueFloat;
            if (RuleType.ComparisonValueType == RuleCompatibleProperty.EntryOption.Enum) TargetValue = Enum.Parse(RuleType.PropertyType, TargetValue);
            
            ActuatorRule actuatorRule = new ActuatorRule()
            {
                ComparisonType = this.ComparisonType,
                TargetValue = this.TargetValue,
                TargetReadingType = this.RuleType.TargetReading,
                ValueOnRule = this.ValueOnRule
            };
            return actuatorRule;
        }
        /// <summary>
        /// Get the RuleTypes for the assembly. First run will be expensive.
        /// </summary>
        /// <returns>A list of RuleType</returns>
        public static List<RuleType> RuleTypes()
        {
            if (_ruleTypesCache != null) return _ruleTypesCache;
            _ruleTypesCache = new List<RuleType>();
            var typesWithSensors = AppDomain.CurrentDomain.GetAssemblies()
                .AsParallel()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.GetCustomAttribute(typeof(ModelHasRuleSensors)) != null);

            foreach (Type type in typesWithSensors)
            {
                var properties = type.GetProperties().Where(x => Attribute.IsDefined(x, typeof(RuleCompatibleProperty)));
                foreach (var property in properties)
                {
                    RuleType ruleType;
                    var propRules = property.GetCustomAttribute<RuleCompatibleProperty>();
                    string propName = property.Name;

                    switch (propRules.Option)
                    {
                        case RuleCompatibleProperty.EntryOption.Options:
                            ruleType = new RuleType
                            {
                                TargetReading = propName,
                                ComparisonTypes = new string[] { "==" },
                                ComparisonValueType = RuleCompatibleProperty.EntryOption.Options,
                                PropertyType = property.GetType(),
                                Options = propRules.Options,
                                Default = propRules.Options[0]
                            };
                            break;
                        case RuleCompatibleProperty.EntryOption.Float:
                            ruleType = new RuleType
                            {
                                TargetReading = propName,
                                ComparisonTypes = new string[] { ">", "<", "==" },
                                ComparisonValueType = RuleCompatibleProperty.EntryOption.Float,
                                PropertyType = property.GetType(),
                                Options = new string[] { },
                                Default = 0f
                            };
                            break;
                        case RuleCompatibleProperty.EntryOption.Bool:
                            ruleType = new RuleType
                            {
                                TargetReading = propName,
                                ComparisonTypes = new string[] { "==" },
                                ComparisonValueType = RuleCompatibleProperty.EntryOption.Bool,
                                PropertyType = property.GetType(),
                                Options = new string[] { "true", "false" },
                                Default = true
                            };
                            break;
                        case RuleCompatibleProperty.EntryOption.Enum:
                            ruleType = new RuleType
                            {
                                TargetReading = propName,
                                ComparisonTypes = new string[] { "==" },
                                ComparisonValueType = RuleCompatibleProperty.EntryOption.Bool,
                                PropertyType = property.GetType(),
                                Options = Enum.GetNames(property.GetType()),
                                Default = Enum.GetNames(property.GetType())[0],
                            };
                            break;

                        default:
                            ruleType = new RuleType
                            {
                                TargetReading = propName,
                                ComparisonTypes = new string[] { ">", "<", "==" },
                                ComparisonValueType = RuleCompatibleProperty.EntryOption.Float,
                                PropertyType = property.GetType(),
                                Options = new string[] { },
                                Default = 0f
                            };
                            break;
                    }
                    _ruleTypesCache.Add(ruleType);
                }
            }
            return _ruleTypesCache;
        }
    }

}
