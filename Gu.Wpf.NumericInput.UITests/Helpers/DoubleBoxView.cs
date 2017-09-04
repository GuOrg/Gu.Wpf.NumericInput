namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using Gu.Wpf.UiAutomation;

    public sealed class DoubleBoxView : IDisposable
    {
        private readonly Application application;

        private bool disposed;

        public DoubleBoxView(string windowName)
        {
            this.application = Application.Launch(Info.CreateStartInfo(windowName));
            this.Window = this.application.MainWindow;
            this.AllowSpinnersBox = this.Window.FindCheckBox("AllowSpinnersBox");
            this.DigitsBox = this.Window.FindTextBox("DigitsBox");
            this.VmValueBox = this.Window.FindTextBox("VmValueBox");
            this.IncrementBox = this.Window.FindTextBox("IncrementBox");
            this.MinBox = this.Window.FindTextBox("MinBox");
            this.MaxBox = this.Window.FindTextBox("MaxBox");
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
        }

        public void Reset()
        {
            this.DigitsBox.Enter(string.Empty);
            this.MinBox.Enter(string.Empty);
            this.MaxBox.Enter(string.Empty);
            this.IncrementBox.Enter("1");
            this.VmValueBox.Enter("0");
            this.Window.FindComboBox("SpinUpdateMode").Select("AsBinding");
            this.Window.FindComboBox("CultureBox").Select("en-US");
            this.Window.FindComboBox("ValidationTriggerBox").Select(ValidationTrigger.PropertyChanged.ToString());
            this.Window.FindCheckBox("AllowLeadingSignBox").IsChecked = true;
            this.Window.FindTextBox("StringFormatBox").Enter(string.Empty);
            this.Window.FindCheckBox("AllowThousandsBox").IsChecked = false;
            this.AllowSpinnersBox.IsChecked = false;
        }
    }
}