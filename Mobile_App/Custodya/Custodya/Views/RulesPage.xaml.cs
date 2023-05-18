using Custodya.Attributes;
using Custodya.Models;
using Custodya.ViewModels;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Custodya.Views;

public partial class RulesPage : ContentPage
{
	private Actuator _actuator;
	public Actuator ActuatorCopy { get; private set; }
	public ObservableCollection<RuleContainer> RuleContainers { get; private set; }
	public List<RuleType> RuleTypes { get; private set; }
	public RulesPage(Actuator actuator)
	{
		Type test = typeof(int);
		_actuator = actuator;
		ActuatorCopy = new Actuator();
        ActuatorCopy.CopyValues(actuator);
		RuleTypes = RuleContainer.RuleTypes();
		RuleContainers = new ObservableCollection<RuleContainer>();
		
		foreach(ActuatorRule rule in ActuatorCopy.Rules)
		{
			if(RuleTypes.Any(x=> x.TargetReading == rule.TargetReadingType))
			{
				RuleType ruleType = RuleTypes.First(x => x.TargetReading == rule.TargetReadingType);
				RuleContainers.Add(new RuleContainer(ruleType, rule));
			}
		}
        InitializeComponent();
        this.BindingContext = this;
    }

    private void DeleteRuleButton_Clicked(object sender, EventArgs e)
    {
		RuleContainer ruleContainer = (sender as Button).CommandParameter as RuleContainer;
		if(ruleContainer != null)
		{
			RuleContainers.Remove(ruleContainer);
		}
    }

    private async void AddRuleButton_Clicked(object sender, EventArgs e)
    {
		string[] options = RuleTypes.Select(x => x.TargetReading).ToArray();
		string sensor = await DisplayActionSheet("Select reading for rule", "Cancel", null, options);
		if(sensor != null && options.Any(x => x == sensor))
		{
			RuleType ruleType = RuleTypes.First(x => x.TargetReading == sensor);
			RuleContainers.Add(new RuleContainer(ruleType));
		}
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
		List<ActuatorRule> rules = new List<ActuatorRule>();
		foreach(RuleContainer container in RuleContainers)
		{
			rules.Add(container.ToRule());
		}
		ActuatorCopy.Rules = rules;
		await App.DeviceTwinService.ApplyChanges(ActuatorCopy);
		await Navigation.PopAsync();
    }
}