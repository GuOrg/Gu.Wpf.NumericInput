namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using Gu.Wpf.NumericInput.Demo.Annotations;

    public class BoxVm<T> : IBoxVm
    {
        private T _value;
        private T _min;
        private T _max;
        private CultureInfo _culture;
        private int? _decimalDigits;
        private bool _allowSpinners;
        private bool _isReadonly;
        private string _suffix;
        private string _regexPattern;

        public BoxVm(Type type)
        {
            this.Type = type;
            this.Configurable = false;
            var instance = Activator.CreateInstance(type);
            //var dps = type.GetFields(BindingFlags.Static | BindingFlags.Public |BindingFlags.FlattenHierarchy)
            //                     .Where(x => x.FieldType == typeof(DependencyProperty))
            //                     .Select(x => (DependencyProperty)x.GetValue(null))
            //                     .ToArray();
            Culture = (CultureInfo)this.DefaultValue(instance, "Culture");
            Min = (T)this.DefaultValue(instance, "MinValue");
            Max = (T)this.DefaultValue(instance, "MaxValue");
            Increment = (T)this.DefaultValue(instance, "Increment");
            DecimalDigits = (int?)this.DefaultValue(instance, "DecimalDigits");
            AllowSpinners = (bool)this.DefaultValue(instance, "AllowSpinners");
            RegexPattern = (string)this.DefaultValue(instance, "RegexPattern");
            Suffix = (string)this.DefaultValue(instance, "Suffix");
        }

        public BoxVm(Type type, T min, T max, T increment)
        {
            this.Type = type;
            this.Configurable = true;
            Min = min;
            Max = max;
            Culture = Cultures.First();
            Increment = increment;
        }

        public Type Type { get; set; }

        public bool Configurable { get; private set; }

        public CultureInfo[] Cultures
        {
            get { return new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("sv-SE") }; }
        }

        public CultureInfo Culture
        {
            get
            {
                return this._culture;
            }
            set
            {
                if (Equals(value, this._culture))
                {
                    return;
                }
                this._culture = value;
                this.OnPropertyChanged();
            }
        }

        public T Min
        {
            get
            {
                return _min;
            }
            set
            {
                if (Equals(value, _min))
                {
                    return;
                }
                _min = value;
                this.OnPropertyChanged();
            }
        }

        IFormattable IBoxVm.Min
        {
            get { return (IFormattable)this.Min; }
            set { this.Min = (T)value; }
        }

        public T Max
        {
            get
            {
                return _max;
            }
            set
            {
                if (Equals(value, _max))
                {
                    return;
                }
                _max = value;
                this.OnPropertyChanged();
            }
        }

        IFormattable IBoxVm.Max
        {
            get { return (IFormattable)this.Max; }
            set { this.Max = (T)value; }
        }

        public T Value
        {
            get { return _value; }
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

        IFormattable IBoxVm.Value
        {
            get { return (IFormattable)this.Value; }
            set { this.Value = (T)value; }
        }

        public T Increment
        {
            get { return _value; }
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

        IFormattable IBoxVm.Increment
        {
            get { return (IFormattable)this.Increment; }
            set { this.Increment = (T)value; }
        }

        public int? DecimalDigits
        {
            get
            {
                return this._decimalDigits;
            }
            set
            {
                if (value == this._decimalDigits)
                {
                    return;
                }
                this._decimalDigits = value;
                this.OnPropertyChanged();
            }
        }

        public bool AllowSpinners
        {
            get
            {
                return this._allowSpinners;
            }
            set
            {
                if (value.Equals(this._allowSpinners))
                {
                    return;
                }
                this._allowSpinners = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsReadonly
        {
            get
            {
                return this._isReadonly;
            }
            set
            {
                if (value.Equals(this._isReadonly))
                {
                    return;
                }
                this._isReadonly = value;
                this.OnPropertyChanged();
            }
        }

        public string Suffix
        {
            get
            {
                return this._suffix;
            }
            set
            {
                if (value == this._suffix)
                {
                    return;
                }
                this._suffix = value;
                this.OnPropertyChanged();
            }
        }

        public string RegexPattern
        {
            get
            {
                return this._regexPattern;
            }
            set
            {
                if (value == this._regexPattern)
                {
                    return;
                }
                this._regexPattern = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            var mode = Configurable ? "Configureble" : "Default";
            return string.Format("{0} ({1})", Type.Name, mode);
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

        private object DefaultValue(object instance, string name)
        {
            var propertyInfos = instance.GetType()
                                        .GetProperties();
            var dependencyProperty = propertyInfos.SingleOrDefault(x => x.Name == name);
            if (dependencyProperty == null || ! dependencyProperty.CanRead)
            {
                return null;
            }
            return dependencyProperty.GetValue(instance);
        }
    }
}
