namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;

    public class ValidationTestsBase : WindowTests
    {
        protected Button LoseFocusButton;
        protected TextBox ViewModelValueBox;
        protected ComboBox CultureBox;

        protected CheckBox AllowLeadingWhiteBox;
        protected CheckBox AllowTrailingWhiteBox;
        protected CheckBox AllowLeadingSignBox;
        protected CheckBox AllowDecimalPointBox;
        protected CheckBox AllowThousandsBox;
        protected CheckBox AllowExponentBox;

        protected override string WindowName { get; } = "DoubleBoxValidationWindow";

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            this.LoseFocusButton = this.Window.GetByText<Button>("lose focus");
            this.ViewModelValueBox = this.Window.Get<TextBox>("ViewModelValue");
            this.CultureBox = this.Window.Get<ComboBox>("Culture");
            this.AllowLeadingWhiteBox = this.Window.Get<CheckBox>("AllowLeadingWhite");
            this.AllowTrailingWhiteBox = this.Window.Get<CheckBox>("AllowTrailingWhite");
            this.AllowLeadingSignBox = this.Window.Get<CheckBox>("AllowLeadingSign");
            this.AllowDecimalPointBox = this.Window.Get<CheckBox>("AllowDecimalPoint");
            this.AllowThousandsBox = this.Window.Get<CheckBox>("AllowThousands");
            this.AllowExponentBox = this.Window.Get<CheckBox>("AllowExponent");
        }
    }
}