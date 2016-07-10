namespace Gu.Wpf.NumericInput
{
    using System;

    public class CanParseValidationResult : NumericValidationResult
    {
        public static readonly NoParameterFormatAndCulture DefaultFormatAndCulture = NoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_valid_number));

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

        public static CanParseValidationResult CreateErrorResult(Type type, string text, IFormatProvider culture)
        {
            var formatAndCulture = DefaultFormatAndCulture.GetOrCreate(culture);
            var message = formatAndCulture.Format;
            return new CanParseValidationResult(type, text, culture, formatAndCulture, false, message);
        }
    }
}