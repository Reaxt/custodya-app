using Custodya.Models;
using Custodya.Views;

namespace Custodya.ControlTemplates;

public partial class ActuatorControlView : ContentView
{
    private Actuator _actuator;
	public ActuatorControlView()
	{
		InitializeComponent();
        _actuator = this.BindingContext as Actuator;
	}
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        _actuator = this.BindingContext as Actuator;
        UpdateUi();
    }
    private void UpdateUi()
    {
        if(_actuator.ControlMethod == Actuator.ControlMethods.manual)
        {
            toggleState.IsEnabled = true;
            EditRulesButton.IsEnabled = false;
            EditRulesButton.IsVisible= false;
            ButtonsGrid.SetColumnSpan(ControlMethodButton, 2);
        } else
        {
            toggleState.IsEnabled = false;
            EditRulesButton.IsEnabled = true;
            EditRulesButton.IsVisible = true;
            ButtonsGrid.SetColumnSpan(ControlMethodButton, 1);
        }
    }

    private void ControlMethodButton_Clicked(object sender, EventArgs e)
    {
        if(_actuator.ControlMethod == Actuator.ControlMethods.manual)
        {
            _actuator.ControlMethod = Actuator.ControlMethods.rules;
        } else
        {
            _actuator.ControlMethod = Actuator.ControlMethods.manual;
        }
        UpdateUi();
    }

    private async void EditRulesButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RulesPage(_actuator));
    }
}