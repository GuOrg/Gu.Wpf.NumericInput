namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;

    public class ValidationTestsBase : WindowTests
    {
        protected override string WindowName { get; } = "DoubleBoxValidationWindow";

        protected Button LoseFocusButton { get; private set; }

        protected TextBox ViewModelValueBox { get; private set; }

        protected ComboBox CultureBox { get; private set; }

        protected CheckBox CanValueBeNullBox { get; private set; }

        protected CheckBox AllowLeadingWhiteBox { get; private set; }

        protected CheckBox AllowTrailingWhiteBox { get; private set; }

        protected CheckBox AllowLeadingSignBox { get; private set; }

        protected CheckBox AllowDecimalPointBox { get; private set; }

        protected CheckBox AllowThousandsBox { get; private set; }

        protected CheckBox AllowExponentBox { get; private set; }

        protected TextBox MaxBox { get; private set; }

        protected TextBox MinBox { get; private set; }

        protected TextBox RegexPatternBox { get; private set; }

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            this.LoseFocusButton = this.Window.GetByText<Button>("lose focus");
            this.ViewModelValueBox = this.Window.Get<TextBox>("ViewModelValue");

            this.CultureBox = this.Window.Get<ComboBox>("Culture");

            this.CanValueBeNullBox = this.Window.Get<CheckBox>("CanValueBeNull");
            this.AllowLeadingWhiteBox = this.Window.Get<CheckBox>("AllowLeadingWhite");
            this.AllowTrailingWhiteBox = this.Window.Get<CheckBox>("AllowTrailingWhite");
            this.AllowLeadingSignBox = this.Window.Get<CheckBox>("AllowLeadingSign");
            this.AllowDecimalPointBox = this.Window.Get<CheckBox>("AllowDecimalPoint");
            this.AllowThousandsBox = this.Window.Get<CheckBox>("AllowThousands");
            this.AllowExponentBox = this.Window.Get<CheckBox>("AllowExponent");

            this.MinBox = this.Window.Get<TextBox>("Min");
            this.MaxBox = this.Window.Get<TextBox>("Max");
            this.RegexPatternBox = this.Window.Get<TextBox>("RegexPattern");
        }
    }
}