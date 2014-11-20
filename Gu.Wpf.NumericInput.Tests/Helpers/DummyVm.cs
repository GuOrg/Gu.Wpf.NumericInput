namespace Gu.Wpf.NumericInput.Tests
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Annotations;

    public class DummyVm<T> : INotifyPropertyChanged
    {
        private T _value = default(T);

        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (Equals(value, _value))
                {
                    return;
                }
                _value = value;
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
