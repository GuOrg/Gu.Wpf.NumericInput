namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Linq;

    internal static class StringHelper
    {
        private static readonly char[] RoundDowns = { '0', '1', '2', '3', '4' };

        internal static bool HasMoreDecimalDigitsThan<T>(this string self, string other, DecimalDigitsBox<T> box)
            where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
        {
            if (box == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(self))
            {
                return false;
            }

            if (self == other)
            {
                return false;
            }

            if (box.CanParse(self) && box.CanParse(other))
            {
                var selfValue = box.Parse(self);
                var otherValue = box.Parse(other);
                var selfRoundtrip = selfValue?.ToString(string.Empty, box.Culture);
                var otherRoundtrip = otherValue?.ToString(string.Empty, box.Culture);
                if (selfRoundtrip == otherRoundtrip)
                {
                    return false;
                }

                if (!selfRoundtrip.StartsWith(otherRoundtrip))
                {
                    return false;
                }

                var c = selfRoundtrip[otherRoundtrip.Length];
                return RoundDowns.Contains(c);
            }

            return false;
        }

        internal static bool HasMoreDecimalDigitsThan<T>(this string self, IFormattable other, DecimalDigitsBox<T> box)
            where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
        {
            return self.HasMoreDecimalDigitsThan(other.ToString(), box);
        }
   }
}
