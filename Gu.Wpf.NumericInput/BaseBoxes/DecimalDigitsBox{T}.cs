namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    public abstract class DecimalDigitsBox<T> : NumericBox<T>
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        /// <summary>
        /// Identifies the Decimals property
        /// </summary>
        public static readonly DependencyProperty DecimalDigitsProperty = DependencyProperty.Register(
            "DecimalDigits",
            typeof(int?),
            typeof(DecimalDigitsBox<T>),
            new PropertyMetadata(
                null,
                OnDecimalsValueChanged));

        protected DecimalDigitsBox(Func<T, T, T> add, Func<T, T, T> subtract)
            : base(add, subtract)
        {
        }

        /// <inheritdoc/>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public int? DecimalDigits
        {
            get { return (int?)this.GetValue(DecimalDigitsProperty); }
            set { this.SetValue(DecimalDigitsProperty, value); }
        }

        protected override void OnCultureChanged(IFormatProvider oldCulture, IFormatProvider newCulture)
        {
            var text = (string)this.GetValue(TextBindableProperty);
            if (string.IsNullOrEmpty(text) || oldCulture == null)
            {
                return;
            }

            T result;
            if (this.TryParse(text, this.NumberStyles, oldCulture, out result))
            {
                var status = this.Status;
                this.Status = NumericInput.Status.Formatting;
                var newText = result.ToString(newCulture);
                if (this.TextSource == TextSource.UserInput)
                {
                    this.SetTextAndCreateUndoAction(newText);
                }
                else
                {
                    this.SetTextClearUndo(newText);
                }

                this.SetCurrentValue(TextBindableProperty, newText);
                this.Status = status;
            }

            base.OnCultureChanged(oldCulture, newCulture);
        }

        private static void OnDecimalsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (DecimalDigitsBox<T>)d;

            // not sure if binding StringFormat to DecimalDigits is nicer
            box.StringFormat = (string)DecimalDigitsToStringFormatConverter.Default.Convert(e.NewValue, null, null, null);
        }
    }
}