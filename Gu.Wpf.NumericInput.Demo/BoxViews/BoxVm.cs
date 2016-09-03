// ReSharper disable ExplicitCallerInfoArgument
namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public abstract class BoxVm<TBox, TValue> : INotifyDataErrorInfo, INotifyPropertyChanged
        where TBox : NumericBox<TValue>
        where TValue : struct, IComparable<TValue>, IFormattable, IConvertible, IEquatable<TValue>
    {
        private TValue? value = default(TValue);
        private TValue? min;
        private TValue? max;
        private IFormatProvider culture;
        private NumberStyles numberStyles;
        private int? decimalDigits;
        private bool allowSpinners;
        private bool isReadOnly;
        private string regexPattern;
        private TValue increment;
        private bool canValueBeNull;
        private string stringFormat;
        private bool hasErrors;
        private ValidationTrigger validationTrigger = ValidationTrigger.PropertyChanged;

        protected BoxVm()
        {
            this.min = DefaultValue(x => x.MinValue);
            this.max = DefaultValue(x => x.MaxValue);
            this.culture = DefaultValue(x => x.Culture);
            this.numberStyles = DefaultValue(x => x.NumberStyles);
            this.decimalDigits = DefaultValue(x => (x as DecimalDigitsBox<TValue>)?.DecimalDigits);
            this.allowSpinners = DefaultValue(x => x.AllowSpinners);
            this.isReadOnly = DefaultValue(x => x.IsReadOnly);
            this.increment = DefaultValue(x => x.Increment);
            this.regexPattern = DefaultValue(x => x.RegexPattern);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public Type Type => typeof(TBox);

        public CultureInfo[] Cultures => new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("sv-SE"), CultureInfo.GetCultureInfo("ja-JP"),  };

        public ValidationTrigger ValidationTrigger
        {
            get { return this.validationTrigger; }
            set
            {
                if (value == this.validationTrigger)
                {
                    return;
                }

                this.validationTrigger = value;
                this.OnPropertyChanged();
            }
        }

        public IFormatProvider Culture
        {
            get
            {
                return this.culture;
            }
            set
            {
                if (Equals(value, this.culture))
                {
                    return;
                }
                this.culture = value;
                this.OnPropertyChanged();
            }
        }

        public NumberStyles NumberStyles
        {
            get { return this.numberStyles; }
            set
            {
                if (value == this.numberStyles)
                {
                    return;
                }

                this.numberStyles = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.AllowLeadingWhite));
                this.OnPropertyChanged(nameof(this.AllowTrailingWhite));
                this.OnPropertyChanged(nameof(this.AllowLeadingSign));
                this.OnPropertyChanged(nameof(this.AllowDecimalPoint));
                this.OnPropertyChanged(nameof(this.AllowExponent));
                this.OnPropertyChanged(nameof(this.AllowThousands));
            }
        }

        public bool AllowLeadingWhite
        {
            get { return this.NumberStyles.HasFlag(NumberStyles.AllowLeadingWhite); }
            set
            {
                if (value)
                {
                    this.NumberStyles |= NumberStyles.AllowLeadingWhite;
                }
                else
                {
                    this.NumberStyles &= ~NumberStyles.AllowLeadingWhite;
                }
            }
        }

        public bool AllowTrailingWhite
        {
            get { return this.NumberStyles.HasFlag(NumberStyles.AllowTrailingWhite); }
            set
            {
                if (value)
                {
                    this.NumberStyles |= NumberStyles.AllowTrailingWhite;
                }
                else
                {
                    this.NumberStyles &= ~NumberStyles.AllowTrailingWhite;
                }
            }
        }

        public bool AllowLeadingSign
        {
            get { return this.NumberStyles.HasFlag(NumberStyles.AllowLeadingSign); }
            set
            {
                if (value)
                {
                    this.NumberStyles |= NumberStyles.AllowLeadingSign;
                }
                else
                {
                    this.NumberStyles &= ~NumberStyles.AllowLeadingSign;
                }
            }
        }

        public bool AllowDecimalPoint
        {
            get { return this.NumberStyles.HasFlag(NumberStyles.AllowDecimalPoint); }
            set
            {
                if (value)
                {
                    this.NumberStyles |= NumberStyles.AllowDecimalPoint;
                }
                else
                {
                    this.NumberStyles &= ~NumberStyles.AllowDecimalPoint;
                }
            }
        }

        public bool AllowExponent
        {
            get { return this.NumberStyles.HasFlag(NumberStyles.AllowExponent); }
            set
            {
                if (value)
                {
                    this.NumberStyles |= NumberStyles.AllowExponent;
                }
                else
                {
                    this.NumberStyles &= ~NumberStyles.AllowExponent;
                }
            }
        }

        public bool AllowThousands
        {
            get { return this.NumberStyles.HasFlag(NumberStyles.AllowThousands); }
            set
            {
                if (value)
                {
                    this.NumberStyles |= NumberStyles.AllowThousands;
                }
                else
                {
                    this.NumberStyles &= ~NumberStyles.AllowThousands;
                }
            }
        }

        public TValue? Min
        {
            get
            {
                return this.min;
            }
            set
            {
                if (Equals(value, this.min))
                {
                    return;
                }
                this.min = value;
                this.OnPropertyChanged();
            }
        }

        public TValue? Max
        {
            get
            {
                return this.max;
            }
            set
            {
                if (Equals(value, this.max))
                {
                    return;
                }

                this.max = value;
                this.OnPropertyChanged();
            }
        }

        public TValue? Value
        {
            get { return this.value; }
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

        public bool CanValueBeNull
        {
            get { return this.canValueBeNull; }
            set
            {
                if (value == this.canValueBeNull)
                {
                    return;
                }

                this.canValueBeNull = value;
                this.OnPropertyChanged();
            }
        }

        public TValue Increment
        {
            get { return this.increment; }
            set
            {
                if (value.Equals(this.increment))
                {
                    return;
                }
                this.increment = value;
                this.OnPropertyChanged();
            }
        }

        public int? DecimalDigits
        {
            get
            {
                return this.decimalDigits;
            }
            set
            {
                if (value == this.decimalDigits)
                {
                    return;
                }
                this.decimalDigits = value;
                this.OnPropertyChanged();
            }
        }

        public string StringFormat
        {
            get { return this.stringFormat; }
            set
            {
                if (value == this.stringFormat) return;
                this.stringFormat = value;
                this.OnPropertyChanged();
            }
        }

        public bool AllowSpinners
        {
            get
            {
                return this.allowSpinners;
            }
            set
            {
                if (value.Equals(this.allowSpinners))
                {
                    return;
                }
                this.allowSpinners = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
            set
            {
                if (value.Equals(this.isReadOnly))
                {
                    return;
                }
                this.isReadOnly = value;
                this.OnPropertyChanged();
            }
        }

        public string RegexPattern
        {
            get
            {
                return this.regexPattern;
            }
            set
            {
                if (value == this.regexPattern)
                {
                    return;
                }
                this.regexPattern = value;
                this.OnPropertyChanged();
            }
        }

        public bool HasErrors
        {
            get { return this.hasErrors; }
            set
            {
                if (value == this.hasErrors) return;
                this.hasErrors = value;
                this.OnPropertyChanged();
                this.OnErrorsChanged(nameof(this.Value));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return this.HasErrors && propertyName == nameof(this.Value)
                ? new[] { "Has error" }
                : Enumerable.Empty<string>();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private static T DefaultValue<T>(Func<TBox, T> property)
        {
            var instance = Activator.CreateInstance<TBox>();
            return property(instance);
        }
    }
}
