namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using TestStack.White;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.UIItems.WindowItems;

    public sealed class DoubleBoxValidationView : IDisposable
    {
        private readonly Application application;
        private bool disposed;

        public DoubleBoxValidationView()
        {
            var windowName = "DoubleBoxValidationWindow";
            this.application = Application.Launch(Info.CreateStartInfo(windowName));
            this.application.WaitWhileBusy();
            this.Window = this.application.GetWindow(windowName);
            this.Window.WaitWhileBusy();
            this.LoseFocusButton = this.Window.GetByText<Button>("lose focus");
            this.ViewModelValueBox = this.Window.Get<TextBox>("ViewModelValue");

            this.StringFormatBox = this.Window.Get<TextBox>("StringFormat");
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
            this.LostFocusValidateOnLostFocusBoxes = new TextBoxAndErrorBox(
                this.Window.Get<TextBox>("LostFocusValidateOnLostFocusBox"),
                this.Window.Get<Label>("LostFocusValidateOnLostFocusBoxError"));

            this.LostFocusValidateOnPropertyChangedBoxes = new TextBoxAndErrorBox(
                this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox"),
                this.Window.Get<Label>("LostFocusValidateOnPropertyChangedBoxError"));

            this.PropertyChangedValidateOnPropertyChangedBoxes = new TextBoxAndErrorBox(
                this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox"),
                this.Window.Get<Label>("PropertyChangedValidateOnPropertyChangedBoxError"));
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
            this.CanValueBeNullBox.Checked = false;

            this.AllowLeadingWhiteBox.Checked = true;
            this.AllowTrailingWhiteBox.Checked = true;
            this.AllowLeadingSignBox.Checked = true;
            this.AllowDecimalPointBox.Checked = true;
            this.AllowThousandsBox.Checked = false;
            this.AllowExponentBox.Checked = true;

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
            this.Window?.Dispose();
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