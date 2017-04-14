namespace Gu.Wpf.NumericInput
{
    using System;

    public class RequiredButMissingValidationResult : NumericValidationResult
    {
        public static readonly NoParameterFormatAndCulture DefaultFormatAndCulture = NoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_number));

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

        public static RequiredButMissingValidationResult CreateErrorResult<T>(string text, NumericBox<T> box)
             where T : struct, IFormattable, IComparable<T>, IConvertible, IEquatable<T>
        {
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