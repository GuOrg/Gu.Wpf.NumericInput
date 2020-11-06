namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when the user input is required but missing.</summary>
    public class RequiredButMissingValidationResult : NumericValidationResult
    {
        /// <summary>Message when user input is required but missing.</summary>
        public static readonly NoParameterFormatAndCulture DefaultFormatAndCulture = NoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_number));

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredButMissingValidationResult"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="text">The <see cref="string"/>.</param>
        /// <param name="currentBoxCulture">The <see cref="IFormatProvider"/>.</param>
        /// <param name="formatAndCulture">The <see cref="NoParameterFormatAndCulture"/>.</param>
        /// <param name="isValid">The <see cref="bool"/>.</param>
        /// <param name="errorContent">The <see cref="object"/>.</param>
        public RequiredButMissingValidationResult(
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

        /// <summary>
        /// Creates a <see cref="RequiredButMissingValidationResult"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="text">The value.</param>
        /// <param name="box">The <see cref="NumericBox{T}"/>.</param>
        /// <returns>A <see cref="RequiredButMissingValidationResult"/>.</returns>
        public static RequiredButMissingValidationResult CreateErrorResult<T>(string text, NumericBox<T> box)
             where T : struct, IFormattable, IComparable<T>, IConvertible, IEquatable<T>
        {
            if (box is null)
            {
                throw new ArgumentNullException(nameof(box));
            }

            var formatAndCulture = DefaultFormatAndCulture.GetOrCreate(box.Culture);
            var message = formatAndCulture.Format;
            return new RequiredButMissingValidationResult(
                type: typeof(T),
                text: text,
                currentBoxCulture: box.Culture,
                formatAndCulture: formatAndCulture,
                isValid: false,
                errorContent: message);
        }
    }
}
