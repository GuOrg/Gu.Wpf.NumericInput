namespace Gu.Wpf.NumericInput.UITests
{
    using System;
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
            this.application = Application.Launch(Info.CreateStartInfo(windowName));
            this.application.WaitWhileBusy();
            this.Window = this.application.GetWindow(windowName);
            this.Window.WaitWhileBusy();
            this.AllowSpinnersBox = this.Window.Get<CheckBox>("AllowSpinnersBox");
            this.DigitsBox = this.Window.Get<TextBox>("DigitsBox");
            this.VmValueBox = this.Window.Get<TextBox>("VmValueBox");
            this.IncrementBox = this.Window.Get<TextBox>("IncrementBox");
            this.MinBox = this.Window.Get<TextBox>("MinBox");
            this.MaxBox = this.Window.Get<TextBox>("MaxBox");
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
            this.Window.Get<ComboBox>("CultureBox").Select("en-US");
            this.Window.Get<ComboBox>("ValidationTriggerBox").Select(ValidationTrigger.PropertyChanged.ToString());
            this.Window.Get<CheckBox>("AllowLeadingSignBox").Checked = true;
            this.Window.Get<TextBox>("StringFormatBox").Enter(string.Empty);
            this.Window.Get<CheckBox>("AllowThousandsBox").Checked = false;
            this.AllowSpinnersBox.Checked = false;
        }
    }
}