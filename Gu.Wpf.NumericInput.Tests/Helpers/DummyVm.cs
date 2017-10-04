﻿namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class DummyVm<T> : INotifyPropertyChanged
        where T : struct, IEquatable<T>
    {
        private T? value;

        public event PropertyChangedEventHandler PropertyChanged;

        public T? Value
        {
            get => this.value;
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
