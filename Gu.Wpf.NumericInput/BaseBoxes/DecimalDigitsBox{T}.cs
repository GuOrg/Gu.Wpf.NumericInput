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
        public static readonly DependencyProperty DecimalDigitsProperty = NumericBox.DecimalDigitsProperty.AddOwner(
            typeof(DecimalDigitsBox<T>),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnDecimalsDigitsChanged));

        /// <summary>
        /// Example:
        /// DecimalDigits="3" sets StringFormat to F3
        /// DecimalDigits="-3" sets StringFormat to 0.###
        /// Defauklt is null.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public int? DecimalDigits
        {
            get { return (int?)this.GetValue(DecimalDigitsProperty); }
            set { this.SetValue(DecimalDigitsProperty, value); }
        }

        protected override void OnCultureChanged(IFormatProvider oldCulture, IFormatProvider newCulture)
        {
            var text = this.Text;
            if (string.IsNullOrEmpty(text) || oldCulture == null)
            {
                return;
            }

            T result;
            if (this.TryParse(text, this.NumberStyles, oldCulture, out result))
            {
                var status = this.Status;
                this.Status = Status.Formatting;
                var newText = result.ToString(newCulture);
                if (this.TextSource == TextSource.UserInput)
                {
                    this.SetTextAndCreateUndoAction(newText);
                }
                else
                {
                    this.SetTextClearUndo(newText);
                }

                this.Status = status;
            }

            base.OnCultureChanged(oldCulture, newCulture);
        }

        private static void OnDecimalsDigitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (DecimalDigitsBox<T>)d;
            var format = (string)DecimalDigitsToStringFormatConverter.Default.Convert(e.NewValue, null, null, null);
            box.SetCurrentValue(StringFormatProperty, format);
        }
    }
}