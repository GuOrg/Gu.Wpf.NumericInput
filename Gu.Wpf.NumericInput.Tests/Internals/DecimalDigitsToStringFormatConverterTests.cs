namespace Gu.Wpf.NumericInput.Tests.Internals
{
    using System.Globalization;
    using NUnit.Framework;

    public class DecimalDigitsToStringFormatConverterTests
    {
        [TestCase(null, 12345.678901, "12345.678901")]
        [TestCase(0, 12345.678901, "12346")]
        [TestCase(1, 12345.678901, "12345.7")]
        [TestCase(3, 12345.678901, "12345.679")]
        [TestCase(9, 12345.678901, "12345.678901000")]
        [TestCase(-3, 12345.678901, "12345.679")]
        [TestCase(-3, 12345.6, "12345.6")]
        [TestCase(3, 12345.6, "12345.600")]
        public void Convert(int? digits, double value, string expected)
        {
            var converter = DecimalDigitsToStringFormatConverter.Default;
            var format = (string?)converter.Convert(digits, null, null, null);
            var actual = value.ToString(format, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected, actual);
        }
    }
}
