namespace Gu.Wpf.NumericInput.Tests
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.NumericInput.Annotations;

    public class DummyVm<T> : INotifyPropertyChanged
    {
        private T _value = default(T);

        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (Equals(value, this._value))
                {
                    return;
                }
                this._value = value;
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
