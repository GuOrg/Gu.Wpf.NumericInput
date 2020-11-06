namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when the user input cannot be parsed.</summary>
    public class CanParseValidationResult : NumericValidationResult
    {
        /// <summary>Message when user typed in text that could not be parsed.</summary>
        public static readonly NoParameterFormatAndCulture PleaseEnterAValidNumberFormatAndCulture = NoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_valid_number));

        /// <summary>
        /// Initializes a new instance of the <see cref="CanParseValidationResult"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="text">The <see cref="string"/>.</param>
        /// <param name="currentBoxCulture">The <see cref="IFormatProvider"/>.</param>
        /// <param name="formatAndCulture">The <see cref="NoParameterFormatAndCulture"/>.</param>
        /// <param name="isValid">The <see cref="bool"/>.</param>
        /// <param name="errorContent">The <see cref="object"/>.</param>
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
            if (box is null)
            {
                throw new ArgumentNullException(nameof(box));
            }

            var formatAndCulture = PleaseEnterAValidNumberFormatAndCulture.GetOrCreate(box.Culture);
            var message = formatAndCulture.Format;
            return new CanParseValidationResult(
                type: typeof(T),
                text: text,
                currentBoxCulture: box.Culture,
                formatAndCulture: formatAndCulture,
                isValid: false,
                errorContent: message);
        }
    }
}
