namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public abstract class BoxVm<TBox, TValue> : IDataErrorInfo
        where TBox : NumericBox<TValue> 
        where TValue : struct, IComparable<TValue>, IFormattable, IConvertible, IEquatable<TValue>
    {
        private TValue value;
        private TValue? min;
        private TValue? max;
        private IFormatProvider culture;
        private int? decimalDigits;
        private bool allowSpinners;
        private bool isReadOnly;
        private string suffix;
        private string regexPattern;
        private TValue increment;

        protected BoxVm()
        {
            min = DefaultValue(x => x.MinValue);
            max = DefaultValue(x => x.MaxValue);
            culture = DefaultValue(x => x.Culture);
            //decimalDigits = DefaultValue(x => x.Dec);
            allowSpinners = DefaultValue(x => x.AllowSpinners);
            isReadOnly = DefaultValue(x => x.IsReadOnly);
            increment = DefaultValue(x => x.Increment);
            suffix = DefaultValue(x => x.Suffix);
            regexPattern = DefaultValue(x => x.RegexPattern);
            increment = DefaultValue(x => x.Increment); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Type Type => typeof(TBox);

        public CultureInfo[] Cultures => new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("sv-SE") };

        public IFormatProvider Culture
        {
            get
            {
                return culture;
            }
            set
            {
                if (Equals(value, culture))
                {
                    return;
                }
                culture = value;
                this.OnPropertyChanged();
            }
        }

        public TValue? Min
        {
            get
            {
                return min;
            }
            set
            {
                if (Equals(value, min))
                {
                    return;
                }
                min = value;
                this.OnPropertyChanged();
            }
        }

        public TValue? Max
        {
            get
            {
                return max;
            }
            set
            {
                if (Equals(value, max))
                {
                    return;
                }
                max = value;
                this.OnPropertyChanged();
            }
        }

        public TValue Value
        {
            get { return value; }
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

        public TValue Increment
        {
            get { return increment; }
            set
            {
                if (value.Equals(increment))
                {
                    return;
                }
                increment = value;
                OnPropertyChanged();
            }
        }

        public int? DecimalDigits
        {
            get
            {
                return decimalDigits;
            }
            set
            {
                if (value == decimalDigits)
                {
                    return;
                }
                decimalDigits = value;
                this.OnPropertyChanged();
            }
        }

        public bool AllowSpinners
        {
            get
            {
                return allowSpinners;
            }
            set
            {
                if (value.Equals(allowSpinners))
                {
                    return;
                }
                allowSpinners = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                if (value.Equals(isReadOnly))
                {
                    return;
                }
                isReadOnly = value;
                this.OnPropertyChanged();
            }
        }

        public string Suffix
        {
            get
            {
                return suffix;
            }
            set
            {
                if (value == suffix)
                {
                    return;
                }
                suffix = value;
                this.OnPropertyChanged();
            }
        }

        public string RegexPattern
        {
            get
            {
                return regexPattern;
            }
            set
            {
                if (value == regexPattern)
                {
                    return;
                }
                regexPattern = value;
                this.OnPropertyChanged();
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Value" && Equals(Value, 3))
                {
                    return "IDataErrorInfo says anything but 3 please!";
                }
                return null;
            }
        }

        public string Error => string.Empty;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static T DefaultValue<T>(Func<TBox, T> property)
        {
            var instance = Activator.CreateInstance<TBox>();
            return property(instance);
        }
    }
}
