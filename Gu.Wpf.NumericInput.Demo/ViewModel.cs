namespace Gu.Wpf.NumericInput.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.NumericInput.Demo.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private double _doubleValue;
        public event PropertyChangedEventHandler PropertyChanged;

        public double DoubleValue
        {
            get { return this._doubleValue; }
            set
            {
                if (value.Equals(this._doubleValue))
                {
                    return;
                }
                this._doubleValue = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
