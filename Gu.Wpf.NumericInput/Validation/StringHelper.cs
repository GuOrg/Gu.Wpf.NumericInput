namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Linq;

    internal static class StringHelper
    {
        private static char[] _roundDowns = new[] { '0', '1', '2', '3', '4' };
        internal static bool HasMoreDecimalDigitsThan(this string self, string other, INumericBox box)
        {
            if (self == other)
            {
                return false;
            }
            if (box.CanParse(self) && box.CanParse(other))
            {
                var selfValue = box.Parse(self);
                var otherValue = box.Parse(other);
                var selfRoundtrip = selfValue.ToString("", box.Culture);
                var otherRoundtrip = otherValue.ToString("", box.Culture);
                if (selfRoundtrip == otherRoundtrip)
                {
                    return false;
                }
                if (!selfRoundtrip.StartsWith(otherRoundtrip))
                {
                    return false;
                }
                var c = selfRoundtrip[otherRoundtrip.Length];
                return _roundDowns.Contains(c);
            }
            return false;
        }

        internal static bool HasMoreDecimalDigitsThan(this string self, IFormattable other, INumericBox box)
        {
            return self.HasMoreDecimalDigitsThan(other.ToString(), box);
        }

        internal static bool HasMoreDecimalDigitsThan(this IFormattable self, IFormattable other, INumericBox box)
        {
            return self.ToString().HasMoreDecimalDigitsThan(other.ToString(), box);
        }

        internal static bool HasMoreDecimalDigitsThan(this IFormattable self, string other, INumericBox box)
        {
            return self.ToString().HasMoreDecimalDigitsThan(other, box);
        }
    }
}
