namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using FlaUI.Core;
    using FlaUI.Core.AutomationElements;
    using FlaUI.Core.Definitions;
    using FlaUI.UIA3;

    public sealed class DoubleBoxValidationView : IDisposable
    {
        private readonly Application application;
        private readonly UIA3Automation automation;

        private bool disposed;

        public DoubleBoxValidationView()
        {
            this.application = Application.Launch(Info.CreateStartInfo("DoubleBoxValidationWindow"));
            this.automation = new UIA3Automation();
            this.Window = this.application.GetMainWindow(this.automation);
            this.LoseFocusButton = this.Window.FindButton("lose focus");
            this.ViewModelValueBox = this.Window.FindTextBox("ViewModelValue");

            this.StringFormatBox = this.Window.FindTextBox("StringFormat");
            this.CultureBox = this.Window.FindComboBox("Culture");

            this.CanValueBeNullBox = this.Window.FindCheckBox("CanValueBeNull");
            this.AllowLeadingWhiteBox = this.Window.FindCheckBox("AllowLeadingWhite");
            this.AllowTrailingWhiteBox = this.Window.FindCheckBox("AllowTrailingWhite");
            this.AllowLeadingSignBox = this.Window.FindCheckBox("AllowLeadingSign");
            this.AllowDecimalPointBox = this.Window.FindCheckBox("AllowDecimalPoint");
            this.AllowThousandsBox = this.Window.FindCheckBox("AllowThousands");
            this.AllowExponentBox = this.Window.FindCheckBox("AllowExponent");

            this.MinBox = this.Window.FindTextBox("Min");
            this.MaxBox = this.Window.FindTextBox("Max");
            this.RegexPatternBox = this.Window.FindTextBox("RegexPattern");
            this.LostFocusValidateOnLostFocusBoxes = new TextBoxAndErrorBox(
                this.Window.FindTextBox("LostFocusValidateOnLostFocusBox"),
                this.Window.FindLabel("LostFocusValidateOnLostFocusBoxError"));

            this.LostFocusValidateOnPropertyChangedBoxes = new TextBoxAndErrorBox(
                this.Window.FindTextBox("LostFocusValidateOnPropertyChangedBox"),
                this.Window.FindLabel("LostFocusValidateOnPropertyChangedBoxError"));

            this.PropertyChangedValidateOnPropertyChangedBoxes = new TextBoxAndErrorBox(
                this.Window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox"),
                this.Window.FindLabel("PropertyChangedValidateOnPropertyChangedBoxError"));
        }

        public Window Window { get; }

        public Button LoseFocusButton { get; }

        public TextBox ViewModelValueBox { get; }

        public TextBox StringFormatBox { get; }

        public ComboBox CultureBox { get; }

        public CheckBox CanValueBeNullBox { get; }

        public CheckBox AllowLeadingWhiteBox { get; }

        public CheckBox AllowTrailingWhiteBox { get; }

        public CheckBox AllowLeadingSignBox { get; }

        public CheckBox AllowDecimalPointBox { get; }

        public CheckBox AllowThousandsBox { get; }

        public CheckBox AllowExponentBox { get; }

        public TextBox MaxBox { get; }

        public TextBox MinBox { get; }

        public TextBox RegexPatternBox { get; }

        public TextBoxAndErrorBox LostFocusValidateOnLostFocusBoxes { get; }

        public TextBoxAndErrorBox LostFocusValidateOnPropertyChangedBoxes { get; }

        public TextBoxAndErrorBox PropertyChangedValidateOnPropertyChangedBoxes { get; }

        public void Reset()
        {
            this.ViewModelValueBox.Text = "0";
            this.CultureBox.Select("en-US");
            this.CanValueBeNullBox.State = ToggleState.Off;

            this.AllowLeadingWhiteBox.State = ToggleState.On;
            this.AllowTrailingWhiteBox.State = ToggleState.On;
            this.AllowLeadingSignBox.State = ToggleState.On;
            this.AllowDecimalPointBox.State = ToggleState.On;
            this.AllowThousandsBox.State = ToggleState.Off;
            this.AllowExponentBox.State = ToggleState.On;

            this.MinBox.Text = string.Empty;
            this.MaxBox.Text = string.Empty;
            this.RegexPatternBox.Enter(string.Empty);
            this.LoseFocusButton.Click();
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.application?.Dispose();
            this.automation.Dispose();
        }

        public class TextBoxAndErrorBox
        {
            public TextBoxAndErrorBox(TextBox doubleBox, Label errorBlock)
            {
                this.DoubleBox = doubleBox;
                this.ErrorBlock = errorBlock;
            }

            public TextBox DoubleBox { get; }

            public Label ErrorBlock { get; }
        }
    }
}