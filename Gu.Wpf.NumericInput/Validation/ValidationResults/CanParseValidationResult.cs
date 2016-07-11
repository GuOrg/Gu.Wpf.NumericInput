namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when the user input cannot be parsed.</summary>
    public class CanParseValidationResult : NumericValidationResult
    {
        public static readonly NoParameterFormatAndCulture PleaseEnterAValidNumberFormatAndCulture = NoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_valid_number));

        public CanParseValidationResult(
            Type type,
            string text,
            IFormatProvider currentBoxCulture,
            NoParameterFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(currentBoxCulture, formatAndCulture, isValid, errorContent)
        {
            this.Type = type;
            this.Text = text;
        }

        /// <summary>Gets the type of the box, i.e. <see cref="double"/> for a <see cref="DoubleBox"/>.</summary>
        public Type Type { get; }

        /// <summary>Gets the text that was found invalid.</summary>
        public string Text { get; }

        public static CanParseValidationResult CreateErrorResult<T>(string text, NumericBox<T> box)
             where T : struct, IFormattable, IComparable<T>, IConvertible, IEquatable<T>
        {
            var formatAndCulture = PleaseEnterAValidNumberFormatAndCulture.GetOrCreate(box.Culture);
            var message = formatAndCulture.Format;
            return new CanParseValidationResult(typeof(T), text, box.Culture, formatAndCulture, false, message);
        }
    }
}