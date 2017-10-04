// ReSharper disable ExplicitCallerInfoArgument
namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public abstract class BoxVm<TBox, TValue> : INotifyDataErrorInfo, INotifyPropertyChanged
        where TBox : NumericBox<TValue>
        where TValue : struct, IComparable<TValue>, IFormattable, IConvertible, IEquatable<TValue>
    {
        private static readonly TBox DefaultValueInstance = Activator.CreateInstance<TBox>();

        private TValue? value = default(TValue);
        private TValue? min;
        private TValue? max;
        private IFormatProvider culture;
        private NumberStyles numberStyles;
        private int? decimalDigits;
        private bool allowSpinners;
        private SpinUpdateMode spinUpdateMode;
        private bool isReadOnly;
        private string regexPattern;
        private TValue increment;
        private bool canValueBeNull;
        private string stringFormat;
        private bool hasErrors;
        private ValidationTrigger validationTrigger = ValidationTrigger.PropertyChanged;

        protected BoxVm()
        {
            this.ResetCommand = new RelayCommand(_ => this.Reset());
            this.Reset();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public Type Type => typeof(TBox);

        public ICommand ResetCommand { get; }

        public IReadOnlyList<CultureInfo> Cultures => new[]
                                         {
                                             CultureInfo.GetCultureInfo("en-US"),
                                             CultureInfo.GetCultureInfo("sv-SE"),
                                             CultureInfo.GetCultureInfo("ja-JP"),
                                         };

        public ValidationTrigger ValidationTrigger
        {
            get => this.validationTrigger;
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
            get => this.culture;
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
            get => this.numberStyles;
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
            get => this.NumberStyles.HasFlag(NumberStyles.AllowLeadingWhite);
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
            get => this.NumberStyles.HasFlag(NumberStyles.AllowTrailingWhite);
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
            get => this.NumberStyles.HasFlag(NumberStyles.AllowLeadingSign);
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
            get => this.NumberStyles.HasFlag(NumberStyles.AllowDecimalPoint);
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
            get => this.NumberStyles.HasFlag(NumberStyles.AllowExponent);
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
            get => this.NumberStyles.HasFlag(NumberStyles.AllowThousands);
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
            get => this.min;
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
            get => this.max;
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
            get => this.value;
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
            get => this.canValueBeNull;
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
            get => this.increment;
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
            get => this.decimalDigits;
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
            get => this.stringFormat;
            set
            {
                if (value == this.stringFormat)
                {
                    return;
                }

                this.stringFormat = value;
                this.OnPropertyChanged();
            }
        }

        public bool AllowSpinners
        {
            get => this.allowSpinners;
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

        public SpinUpdateMode SpinUpdateMode
        {
            get => this.spinUpdateMode;

            set
            {
                if (value == this.spinUpdateMode)
                {
                    return;
                }

                this.spinUpdateMode = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsReadOnly
        {
            get => this.isReadOnly;
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
            get => this.regexPattern;
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
            get => this.hasErrors;
            set
            {
                if (value == this.hasErrors)
                {
                    return;
                }

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
            return property(DefaultValueInstance);
        }

        private void Reset()
        {
            this.value = default(TValue);
            this.min = DefaultValue(x => x.MinValue);
            this.max = DefaultValue(x => x.MaxValue);
            this.culture = DefaultValue(x => x.Culture);
            this.numberStyles = DefaultValue(x => x.NumberStyles);
            this.decimalDigits = DefaultValue(x => (x as DecimalDigitsBox<TValue>)?.DecimalDigits);
            this.allowSpinners = DefaultValue(x => x.AllowSpinners);
            this.spinUpdateMode = DefaultValue(x => x.SpinUpdateMode);
            this.isReadOnly = DefaultValue(x => x.IsReadOnly);
            this.regexPattern = DefaultValue(x => x.RegexPattern);
            this.increment = DefaultValue(x => x.Increment);
            this.canValueBeNull = DefaultValue(x => x.CanValueBeNull);
            this.stringFormat = DefaultValue(x => x.StringFormat);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
