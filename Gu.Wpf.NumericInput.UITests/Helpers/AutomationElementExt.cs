namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using FlaUI.Core.AutomationElements;
    using FlaUI.Core.AutomationElements.Infrastructure;
    using FlaUI.Core.Conditions;
    using FlaUI.Core.Definitions;

    public static class AutomationElementExt
    {
        public static AutomationElement Parent(this AutomationElement element)
        {
            return element.Automation
                          .TreeWalkerFactory
                          .GetRawViewWalker()
                          .GetParent(element);
        }

        public static bool WaitUntilResponsive(this AutomationElement automationElement)
        {
            FlaUI.Core.Input.Helpers.WaitUntilInputIsProcessed();
            return FlaUI.Core.Input.Helpers.WaitUntilResponsive(automationElement);
        }

        [Obsolete("Temporary")]
        public static T Get<T>(this AutomationElement parent, string name)
            where T : AutomationElement
        {
            throw new NotImplementedException();
        }

        public static Button FindButton(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.Button)
                         .AsButton();
        }

        public static AutomationElement FindGroupBox(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.Group);
        }

        public static TextBox FindTextBox(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.Edit)
                         .AsTextBox();
        }

        public static Label FindLabel(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.Text)
                         .AsLabel();
        }

        public static CheckBox FindCheckBox(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.CheckBox)
                         .AsCheckBox();
        }

        public static ComboBox FindComboBox(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.ComboBox)
                         .AsComboBox();
        }

        public static Slider FindSlider(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.Slider)
                         .AsSlider();
        }

        public static Grid FindListBox(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.List)
                         .AsGrid();
        }

        public static AutomationElement FindByNameOrId(this AutomationElement parent, string name, ControlType controlType)
        {
            return parent.FindFirstDescendant(
                new AndCondition(
                    parent.ConditionFactory.ByControlType(controlType),
                    new OrCondition(
                        parent.ConditionFactory.ByName(name),
                        parent.ConditionFactory.ByAutomationId(name))));
        }

        public static AutomationElement FindByNameOrId(this AutomationElement parent, string name)
        {
            return parent.FindFirstDescendant(
                new OrCondition(
                    parent.ConditionFactory.ByName(name),
                    parent.ConditionFactory.ByAutomationId(name)));
        }
    }
}