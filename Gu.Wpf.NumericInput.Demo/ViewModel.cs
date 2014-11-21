namespace Gu.Wpf.NumericInput.Demo
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private double _doubleValue;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public CultureInfo[] Cultures
        {
            get { return new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("sv-SE") }; }
        }

        public double DoubleValue
        {
            get { return _doubleValue; }
            set
            {
                if (value.Equals(_doubleValue))
                {
                    return;
                }
                _doubleValue = value;
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
