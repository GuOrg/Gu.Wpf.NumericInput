namespace Gu.Wpf.NumericInput.Demo.UiTestWindows
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class CultureVm : INotifyPropertyChanged
    {
        private double value = 1.234;
        private CultureInfo culture = CultureInfo.GetCultureInfo("sv-se");

        public event PropertyChangedEventHandler PropertyChanged;

        public double Value
        {
            get => this.value;
            set
            {
                if (value.Equals(this.value))
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        public CultureInfo Culture
        {
            get => this.culture;
            set
            {
                if (Equals(value, this.culture))
                {
                    return;
                }

                this.culture = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.CultureName));
            }
        }

        public string CultureName => this.Culture.Name;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
