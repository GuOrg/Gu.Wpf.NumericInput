namespace Gu.Wpf.NumericInput.Demo
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.NumericInput.Demo.Annotations;

    public class NullbableDoubleViewModel : INotifyPropertyChanged
    {
        private CultureInfo _culture;

        private double? _value;

        private double? _max;

        private double? _min;

        private bool _showSpinners;

        public event PropertyChangedEventHandler PropertyChanged;


        public CultureInfo[] Cultures
        {
            get { return new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("sv-SE") }; }
        }

        public CultureInfo Culture
        {
            get
            {
                return _culture;
            }
            set
            {
                if (Equals(value, _culture))
                {
                    return;
                }
                _culture = value;
                this.OnPropertyChanged();
            }
        }

        public double? Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.Equals(_value))
                {
                    return;
                }
                _value = value;
                OnPropertyChanged();
            }
        }

        public double? Max
        {
            get
            {
                return _max;
            }
            set
            {
                if (value.Equals(_max))
                {
                    return;
                }
                _max = value;
                OnPropertyChanged();
            }
        }

        public double? Min
        {
            get
            {
                return _min;
            }
            set
            {
                if (value.Equals(_min))
                {
                    return;
                }
                _min = value;
                OnPropertyChanged();
            }
        }

        public bool ShowSpinners
        {
            get
            {
                return _showSpinners;
            }
            set
            {
                if (value == _showSpinners)
                {
                    return;
                }
                _showSpinners = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
