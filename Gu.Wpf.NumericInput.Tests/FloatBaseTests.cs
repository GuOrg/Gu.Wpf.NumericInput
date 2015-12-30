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
            this.Sut.Text = "1";
            Assert.AreEqual(1, this.Sut.Value);
            Assert.AreEqual("1", this.Sut.Text);

            this.Sut.Text = "1.";
            Assert.AreEqual(1, this.Sut.Value);
            Assert.AreEqual("1.", this.Sut.Text);

            this.Sut.Text = "1.0";
            Assert.AreEqual(1, this.Sut.Value);
            Assert.AreEqual("1.0", this.Sut.Text);
        }

        [TestCase("sv-SE", "1,23", "en-US", "1.23")]
        [TestCase("en-US", "1.23", "sv-SE", "1,23")]
        [TestCase("en-US", "1.23e", "sv-SE", "1.23e")]
        public void Culture(string culture1, string text, string culture2, string expected)
        {
            this.Sut.Culture = new CultureInfo(culture1);
            this.Sut.Text = text;
            this.Sut.Culture = new CultureInfo(culture2);
            Assert.AreEqual(expected, this.Sut.Text);
        }

        [TestCase(2, "1.234", "1.23", "1.234")]
        public void ValueNotAffectedByDecimalDigits(int decimals, string text, string expectedText, string expected)
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 3);
            this.Sut.Text = text;
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals);
            Assert.AreEqual(expectedText, this.Sut.Text);
            var actual = this.Sut.Value.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expected, actual); // Comparing strings cos conversion issue
        }

        [TestCase("1.234", "1.234", "1.23", "1.23")]
        public void ValueUpdatedOnFewerDecimalDigitsFromUser(string text1, string expected1, string text2, string expected2)
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 5);

            this.Sut.Text = text1;
            var actual = this.Sut.Value.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expected1, actual);

            this.Sut.Text = text2;
            var actual2 = this.Sut.Value.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expected2, actual2);
        }

        [TestCase("1.234", 2, "1.23", 4, "1.2340")]
        public void RoundtripDecimals(string text, int decimals1, string expected1, int decimals2, string expected2)
        {
            this.Sut.Text = text;
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals1);
            Assert.AreEqual(expected1, this.Sut.Text);
            Assert.AreEqual(text, this.Sut.Value.Value.ToString(CultureInfo.InvariantCulture));

            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals2);
            Assert.AreEqual(expected2, this.Sut.Text);
        }

        [Test]
        public void AddedDigitsNotTruncated()
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 2);
            this.Sut.Text = "1.23";
            this.Sut.Text = "1.234";
            var actual = this.Sut.Value.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual("1.234", actual);
        }

        [Test]
        public void FewerDecimalsUpdatesValue()
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 4);
            this.Sut.Text = "1.2334";
            this.Sut.Text = "1.23";
            var actual = this.Sut.Value.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual("1.23", actual);
        }

        [TestCase("sv-SE", "1,23", "en-US", "1.2", "1.23")]
        [TestCase("en-US", "1.23", "sv-SE", "1,2", "1.23")]
        [TestCase("en-US", "1.23e", "sv-SE", "1.23e", "0")]
        public void ChangeCultureDoesNotTruncDecimals(string culture1, string text, string culture2, string expected, string expectedValue)
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 1);
            this.Sut.Culture = new CultureInfo(culture1);
            this.Sut.Text = text;
            var value1 = this.Sut.Value.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expectedValue, value1);

            this.Sut.Culture = new CultureInfo(culture2);
            Assert.AreEqual(expected, this.Sut.Text);
            var value2 = this.Sut.Value.Value.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(expectedValue, value2);
        }
    }
}
