namespace Gu.Wpf.NumericInput.Tests
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using JetBrains.Annotations;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
