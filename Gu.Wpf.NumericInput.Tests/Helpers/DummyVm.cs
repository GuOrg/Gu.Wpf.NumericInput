namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using JetBrains.Annotations;

    public class DummyVm<T> : INotifyPropertyChanged
        where T : struct, IEquatable<T>
    {
        private T? value = default(T);

        public event PropertyChangedEventHandler PropertyChanged;

        public T? Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (Equals(value, this.value))
                {
                    return;
                }
                this.value = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
