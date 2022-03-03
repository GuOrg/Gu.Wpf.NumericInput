namespace Gu.Wpf.NumericInput.Demo.Windows
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class CultureVm : INotifyPropertyChanged
    {
        private const double DefaultValue = 1.234;
        private static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("sv-se");

        private double value = DefaultValue;
        private CultureInfo culture = DefaultCulture;

        public CultureVm()
        {
            this.ResetCommand = new RelayCommand(_ =>
            {
                this.Value = DefaultValue;
                this.Culture = DefaultCulture;
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string CultureName => this.Culture.Name;

        public ICommand ResetCommand { get; }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
