namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when the user input does not match a regex pattern.</summary>
    public class RegexValidationResult : NumericValidationResult
    {
        public static readonly NoParameterFormatAndCulture PleaseProvideValidInputFormatAndCulture = NoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_provide_valid_input));
        public static readonly NoParameterFormatAndCulture SyntaxErrorInRegexPatternFormatAndCulture = NoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Syntax_error_in_regex_pattern));

        public RegexValidationResult(
            string text,
            string? pattern,
            Exception? exception,
            IFormatProvider currentBoxCulture,
            NoParameterFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(currentBoxCulture, formatAndCulture, isValid, errorContent)
        {
            this.Text = text;
            this.Pattern = pattern;
            this.Exception = exception;
        }

        /// <summary>Gets the text that was found invalid.</summary>
        public string Text { get; }

        /// <summary>Gets the regex pattern that was used for validation.</summary>
        public string? Pattern { get; }

        /// <summary>
        /// Gets the <see cref="Exception"/> that was thrown during <see cref="System.Text.RegularExpressions.Regex.Match(string, string)"/> if any.
        /// Null if no exception was thrown.
        /// </summary>
        public Exception? Exception { get; }

        public static RegexValidationResult CreateErrorResult(string text, BaseBox box)
        {
            if (box is null)
            {
                throw new ArgumentNullException(nameof(box));
            }

            var formatAndCulture = PleaseProvideValidInputFormatAndCulture.GetOrCreate(box.Culture);
            var message = formatAndCulture.Format;
            return new RegexValidationResult(
                text: text,
                pattern: box.RegexPattern,
                exception: null,
                currentBoxCulture: box.Culture,
                formatAndCulture: formatAndCulture,
                isValid: false,
                errorContent: message);
        }

        public static RegexValidationResult CreateMalformedPatternErrorResult(string text, Exception exception, BaseBox box)
        {
            if (box is null)
            {
                throw new ArgumentNullException(nameof(box));
            }

            var formatAndCulture = SyntaxErrorInRegexPatternFormatAndCulture.GetOrCreate(box.Culture);
            var message = formatAndCulture.Format;
            return new RegexValidationResult(
                text: text,
                pattern: box.RegexPattern,
                exception: exception,
                currentBoxCulture: box.Culture,
                formatAndCulture: formatAndCulture,
                isValid: false,
                errorContent: message);
        }
    }
}
