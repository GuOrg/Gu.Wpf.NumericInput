using System;

namespace Gu.Wpf.NumericInput.Tests
{
    using System.Globalization;
    using NUnit.Framework;

    public abstract class FloatBaseTests<TBox, T> : NumericBoxTests<TBox, T>
        where TBox : NumericBox<T>
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        [Test]
        public void AppendDecimalDoesNotTruncateText()
        {
            Sut.Text = "1";
            Assert.AreEqual(1, Sut.Value);
            Assert.AreEqual("1", Sut.Text);

            Sut.Text = "1.";
            Assert.AreEqual(1, Sut.Value);
            Assert.AreEqual("1.", Sut.Text);

            Sut.Text = "1.0";
            Assert.AreEqual(1, Sut.Value);
            Assert.AreEqual("1.0", Sut.Text);
        }

        [TestCase("sv-SE", "1,23", "en-US", "1.23")]
        [TestCase("en-US", "1.23", "sv-SE", "1,23")]
        [TestCase("en-US", "1.23e", "sv-SE", "1.23e")]
        public void Culture(string culture1, string text, string culture2, string expected)
        {
            Sut.Culture = new CultureInfo(culture1);
            Sut.Text = text;
            Sut.Culture = new CultureInfo(culture2);
            Assert.AreEqual(expected, Sut.Text);
        }

        [TestCase(2, "1.234", "1.23", "1.234")]
        public void ValueNotAffectedByDecimalDigits(int decimals, string text, string expectedText, string expected)
        {
            Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 3);
            Sut.Text = text;
            Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals);
            Assert.AreEqual(expectedText, Sut.Text);
            var actual = Sut.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expected, actual); // Comparing strings cos conversion issue
        }

        [TestCase("1.234", "1.234", "1.23", "1.23")]
        public void ValueUpdatedOnFewerDecimalDigitsFromUser(string text1, string expected1, string text2, string expected2)
        {
            Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 5);

            Sut.Text = text1;
            var actual = Sut.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expected1, actual);

            Sut.Text = text2;
            var actual2 = Sut.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expected2, actual2);
        }

        [TestCase("1.234", 2, "1.23", 4, "1.2340")]
        public void RoundtripDecimals(string text, int decimals1, string expected1, int decimals2, string expected2)
        {
            Sut.Text = text;
            Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals1);
            Assert.AreEqual(expected1, Sut.Text);
            Assert.AreEqual(text, Sut.Value.ToString(CultureInfo.InvariantCulture));

            Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals2);
            Assert.AreEqual(expected2, Sut.Text);
        }

        [Test]
        public void AddedDigitsNotTruncated()
        {
            Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 2);
            Sut.Text = "1.23";
            Sut.Text = "1.234";
            var actual = Sut.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual("1.234", actual);
        }

        [Test]
        public void FewerDecimalsUpdatesValue()
        {
            Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 4);
            Sut.Text = "1.2334";
            Sut.Text = "1.23";
            var actual = Sut.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual("1.23", actual);
        }

        [TestCase("sv-SE", "1,23", "en-US", "1.2", "1.23")]
        [TestCase("en-US", "1.23", "sv-SE", "1,2", "1.23")]
        [TestCase("en-US", "1.23e", "sv-SE", "1.23e", "0")]
        public void ChangeCultureDoesNotTruncDecimals(string culture1, string text, string culture2, string expected, string expectedValue)
        {
            Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 1);
            Sut.Culture = new CultureInfo(culture1);
            Sut.Text = text;
            var value1 = Sut.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expectedValue, value1);

            Sut.Culture = new CultureInfo(culture2);
            Assert.AreEqual(expected, Sut.Text);
            var value2 = Sut.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expectedValue, value2);
        }
    }
}
