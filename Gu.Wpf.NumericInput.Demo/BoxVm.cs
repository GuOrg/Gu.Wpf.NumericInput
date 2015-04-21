namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

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
        private T _increment;

        public BoxVm(Type type)
        {
            Type = type;
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
            : this(type)
        {
            this.Configurable = true;
            Min = min;
            Max = max;
            Culture = Cultures.First();
            Increment = increment;
        }

        public Type Type { get; private set; }

        public bool Configurable { get; private set; }

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
                this.OnPropertyChanged();
            }
        }

        IFormattable IBoxVm.Value
        {
            get { return (IFormattable)this.Value; }
            set { this.Value = (T)value; }
        }

        public T Increment
        {
            get { return _increment; }
            set
            {
                if (value.Equals(_increment))
                {
                    return;
                }
                _increment = value;
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
                return _decimalDigits;
            }
            set
            {
                if (value == _decimalDigits)
                {
                    return;
                }
                _decimalDigits = value;
                this.OnPropertyChanged();
            }
        }

        public bool AllowSpinners
        {
            get
            {
                return _allowSpinners;
            }
            set
            {
                if (value.Equals(_allowSpinners))
                {
                    return;
                }
                _allowSpinners = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsReadonly
        {
            get
            {
                return _isReadonly;
            }
            set
            {
                if (value.Equals(_isReadonly))
                {
                    return;
                }
                _isReadonly = value;
                this.OnPropertyChanged();
            }
        }

        public string Suffix
        {
            get
            {
                return _suffix;
            }
            set
            {
                if (value == _suffix)
                {
                    return;
                }
                _suffix = value;
                this.OnPropertyChanged();
            }
        }

        public string RegexPattern
        {
            get
            {
                return _regexPattern;
            }
            set
            {
                if (value == _regexPattern)
                {
                    return;
                }
                _regexPattern = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            var mode = Configurable ? "Configurable" : "Default";
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
            if (dependencyProperty == null || !dependencyProperty.CanRead)
            {
                return null;
            }
            return dependencyProperty.GetValue(instance);
        }
    }
}
