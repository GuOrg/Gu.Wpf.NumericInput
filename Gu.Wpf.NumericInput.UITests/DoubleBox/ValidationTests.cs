namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;

    public class ValidationTestsBase : WindowTests
    {
        protected Button LoseFocusButton;
        protected TextBox ViewModelValueBox;
        protected ComboBox CultureBox;
        protected CheckBox AllowDecimalPointBox;

        protected override string WindowName { get; } = "DoubleBoxValidationWindow";

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            this.LoseFocusButton = this.Window.GetByText<Button>("lose focus");
            this.ViewModelValueBox = this.Window.Get<TextBox>("ViewModelValue");
            this.CultureBox = this.Window.Get<ComboBox>("Culture");
            this.AllowDecimalPointBox = this.Window.Get<CheckBox>("AllowDecimalPoint");
        }
    }
}