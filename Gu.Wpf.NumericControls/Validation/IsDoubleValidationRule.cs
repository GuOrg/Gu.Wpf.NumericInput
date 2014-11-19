namespace Gu.Wpf.NumericControls.Validation
{
    using System.Globalization;
    using System.Windows.Controls;

    public class IsDoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is double)
            {
                return ValidationResult.ValidResult;
            }
            var s = (string)value;
            double temp;
            if (double.TryParse(s, NumberStyles.Float, cultureInfo, out temp))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, string.Format("Cannot parse '{0}' to a double", s));
        }
    }
}
