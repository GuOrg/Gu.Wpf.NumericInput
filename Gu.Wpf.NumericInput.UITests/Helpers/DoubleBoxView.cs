namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using Gu.Wpf.NumericInput.Demo;
    using TestStack.White;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.UIItems.WindowItems;

    public sealed class DoubleBoxView : IDisposable
    {
        private readonly Application application;
        private bool disposed;

        public DoubleBoxView(string windowName)
        {
            this.application = Application.AttachOrLaunch(Info.CreateStartInfo(windowName));
            this.application.WaitWhileBusy();
            this.Window = this.application.GetWindow(windowName);
            this.Window.WaitWhileBusy();
            this.AllowSpinnersBox = this.Window.Get<CheckBox>(AutomationIds.AllowSpinnersBox);
            this.DigitsBox = this.Window.Get<TextBox>(AutomationIds.DigitsBox);
            this.VmValueBox = this.Window.Get<TextBox>(AutomationIds.VmValueBox);
            this.IncrementBox = this.Window.Get<TextBox>(AutomationIds.IncrementBox);
            this.MinBox = this.Window.Get<TextBox>(AutomationIds.MinBox);
            this.MaxBox = this.Window.Get<TextBox>(AutomationIds.MaxBox);
        }

        public Window Window { get; }

        public CheckBox AllowSpinnersBox { get; }

        public TextBox DigitsBox { get; }

        public TextBox VmValueBox { get; }

        public TextBox IncrementBox { get; }

        public TextBox MinBox { get; }

        public TextBox MaxBox { get; }

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

        public void Reset()
        {
            this.DigitsBox.Enter(string.Empty);
            this.MinBox.Enter(string.Empty);
            this.MaxBox.Enter(string.Empty);
            this.IncrementBox.Enter("1");
            this.VmValueBox.Enter("0");
            this.Window.Get<ComboBox>(AutomationIds.CultureBox).Select("en-US");
            this.Window.Get<ComboBox>(AutomationIds.ValidationTriggerBox).Select(ValidationTrigger.PropertyChanged.ToString());
            this.Window.Get<CheckBox>(AutomationIds.AllowLeadingSignBox).Checked = true;
            this.Window.Get<TextBox>(AutomationIds.StringFormatBox).Enter(string.Empty);
            this.Window.Get<CheckBox>(AutomationIds.AllowThousandsBox).Checked = false;
            this.AllowSpinnersBox.Checked = false;
        }
    }
}