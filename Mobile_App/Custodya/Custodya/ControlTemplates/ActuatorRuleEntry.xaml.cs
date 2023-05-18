using Custodya.ViewModels;

namespace Custodya.ControlTemplates;

public partial class ActuatorRuleEntry : ContentView
{
	public ActuatorRuleEntry()
	{
		InitializeComponent();
	}
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        RuleContainer rule = this.BindingContext as RuleContainer;
        switch (rule.RuleType.ComparisonValueType) 
        {
            case Attributes.RuleCompatibleProperty.EntryOption.Float:
                TargetValueFloatEntry.IsEnabled = true;
                TargetValueFloatEntry.IsVisible = true;
                break;
            case Attributes.RuleCompatibleProperty.EntryOption.Bool:
                TargetValueBoolSwitch.IsEnabled = true;
                TargetValueBoolSwitch.IsVisible = true;
                break;
            case Attributes.RuleCompatibleProperty.EntryOption.Options:
                TargetValueOptionPicker.IsEnabled = true;
                TargetValueOptionPicker.IsVisible = true;
                break;
        }
    }
}