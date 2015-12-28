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
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                OnDecimalsValueChanged));

        protected DecimalDigitsBox(Func<T, T, T> add, Func<T, T, T> subtract)
            : base(add, subtract)
        {
        }

        /// <inheritdoc/>
        [Category("NumericBox")]
        [Browsable(true)]
        public int? DecimalDigits
        {
            get { return (int?)this.GetValue(DecimalDigitsProperty); }
            set { this.SetValue(DecimalDigitsProperty, value); }
        }

        private static void OnDecimalsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (DecimalDigitsBox<T>)d;

            // not sure if binding StringFormat to DecimalDigits is nicer
            box.StringFormat = (string)DecimalDigitsToStringFormatConverter.Default.Convert(e.NewValue, null, null, null);
        }
    }
}