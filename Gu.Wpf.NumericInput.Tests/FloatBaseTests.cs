#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
#pragma warning disable WPF0014 // SetValue must use registered type.
namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Controls;
    using Gu.Wpf.NumericInput.Tests.Internals;
    using NUnit.Framework;

    public abstract class FloatBaseTests<TBox, T> : NumericBoxTests<TBox, T>
        where TBox : DecimalDigitsBox<T>
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        [Test]
        public void AppendDecimalDoesNotTruncateText()
        {
            this.Box.Text = "1";
            Assert.AreEqual(1, this.Box.Value);
            Assert.AreEqual("1", this.Box.Text);

            this.Box.Text = "1.";
            Assert.AreEqual(1, this.Box.Value);
            Assert.AreEqual("1.", this.Box.Text);

            this.Box.Text = "1.0";
            Assert.AreEqual(1, this.Box.Value);
            Assert.AreEqual("1.0", this.Box.Text);
        }

        [TestCase("sv-SE", "1,23", "en-US", "1.23")]
        [TestCase("en-US", "1.23", "sv-SE", "1,23")]
        [TestCase("en-US", "1.23e", "sv-SE", "1.23e")]
        public void Culture(string culture1, string text, string culture2, string expected)
        {
            this.Box.Culture = new CultureInfo(culture1);
            this.Box.Text = text;
            this.Box.Culture = new CultureInfo(culture2);
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
            Assert.AreEqual(expected, this.Box.Text);
            this.Box.RaiseLostFocus();
            Assert.AreEqual(expected, this.Box.FormattedText);
        }

        [TestCase(2, "1.234", "1.23", "1.234")]
        public void ValueNotAffectedByDecimalDigits(int decimals, string text, string expectedText, string expectedValue)
        {
            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 3);
            this.Box.Text = text;
            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals);
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
            Assert.AreEqual(text, this.Box.Text);
            this.Box.RaiseLostFocus();
            Assert.AreEqual(expectedText, this.Box.FormattedText);
            Assert.AreEqual(expectedValue, this.Box.Value.ToString());
        }

        [TestCase(2, 3, "1.234", "1.23", "1.234")]
        [TestCase(3, 2, "1.234", "1.234", "1.23")]
        public void DecimalDigitsWhenValueFromDataContext(int decimals1, int decimals2, string text, string expectedText1, string expectedText2)
        {
            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals1);
            var value = this.Box.Parse(text);
            var statuses = new List<Status>();
            var expectedStatuses = new List<Status>();
            var sources = new List<TextSource>();
            using (this.Box.PropertyChanged(BaseBox.StatusProperty, x => statuses.Add((Status)x.NewValue)))
            using (this.Box.PropertyChanged(BaseBox.TextSourceProperty, x => sources.Add((TextSource)x.NewValue)))
            {
                this.Vm.Value = value;
                Assert.AreEqual(text, this.Box.Text);
                Assert.AreEqual(expectedText1, this.Box.FormattedText);
                Assert.AreEqual(value, this.Box.Value);
                expectedStatuses.AddRange(new[] { Status.UpdatingFromValueBinding, Status.Validating, Status.UpdatingFromValueBinding, Status.Idle });
                CollectionAssert.AreEqual(expectedStatuses, statuses);
                CollectionAssert.AreEqual(new[] { TextSource.ValueBinding }, sources);

                this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals2);
                Assert.AreEqual(Status.Idle, this.Box.Status);
                Assert.AreEqual(TextSource.ValueBinding, this.Box.TextSource);
                Assert.AreEqual(text, this.Box.Text);
                Assert.AreEqual(expectedText2, this.Box.FormattedText);
                Assert.AreEqual(value, this.Box.Value);
                expectedStatuses.AddRange(new[] { Status.Validating, Status.Idle,  });
                CollectionAssert.AreEqual(expectedStatuses, statuses);
                CollectionAssert.AreEqual(new[] { TextSource.ValueBinding }, sources);

                this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals1);
                Assert.AreEqual(text, this.Box.Text);
                Assert.AreEqual(expectedText1, this.Box.FormattedText);
                Assert.AreEqual(value, this.Box.Value);
                CollectionAssert.AreEqual(new[] { TextSource.ValueBinding }, sources);
            }
        }

        [TestCase(2, 1, "1.234", "1.23")]
        public void DigitsUpdatesWhenGreaterThanMax(int decimals, T max, string text, string expectedText)
        {
            this.Box.Text = text;
            this.Box.MaxValue = max;
            Assert.AreEqual(true, Validation.GetHasError(this.Box));

            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals);
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
            Assert.AreEqual(text, this.Box.Text);
            Assert.AreEqual(expectedText, this.Box.FormattedText);
            Assert.AreEqual(null, this.Box.Value);
            Assert.AreEqual(true, Validation.GetHasError(this.Box));
        }

        [TestCase("1.234", "1.234", "1.23", "1.23")]
        public void ValueUpdatedOnFewerDecimalDigitsFromUser(string text1, string expected1, string text2, string expected2)
        {
            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 5);
            this.Box.Text = text1;
            var actual = this.Box.Value.ToString();
            Assert.AreEqual(expected1, actual);
            this.Box.Text = text2;
            var actual2 = this.Box.Value.ToString();
            Assert.AreEqual(expected2, actual2);
        }

        [TestCase("1.234", 2, "1.23", 4, "1.2340")]
        public void RoundtripDecimals(string text, int decimals1, string expected1, int decimals2, string expected2)
        {
            this.Box.Text = text;
            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals1);
            Assert.AreEqual(text, this.Box.Text);
            Assert.AreEqual(expected1, this.Box.FormattedText);
            Assert.AreEqual(text, this.Box.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);

            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals2);
            Assert.AreEqual(text, this.Box.Text);
            Assert.AreEqual(expected2, this.Box.FormattedText);
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
        }

        [Test]
        public void AddedDigitsNotTruncated()
        {
            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 2);
            this.Box.Text = "1.23";
            this.Box.Text = "1.234";
            Assert.AreEqual("1.234", this.Box.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
        }

        [Test]
        public void FewerDecimalsUpdatesValue()
        {
            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 4);
            this.Box.Text = "1.2334";
            this.Box.Text = "1.23";
            Assert.AreEqual("1.23", this.Box.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
        }

        [TestCase("sv-SE", "1,23", "en-US", "1.23", "1.2", "1.23")]
        [TestCase("en-US", "1.23", "sv-SE", "1,23", "1,2", "1.23")]
        [TestCase("en-US", "1.23e", "sv-SE", "1.23e", "1.23e", "")]
        public void ChangeCultureDoesNotTruncateDecimals(string culture1, string text, string culture2, string expectedText, string expectedFormattedText, string expectedValue)
        {
            this.Box.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 1);
            this.Box.Culture = new CultureInfo(culture1);
            this.Box.Text = text;
            Assert.AreEqual(text, this.Box.Text);
            this.Box.UpdateFormattedText();
            ////Assert.AreEqual(text, this.Box.FormattedText);
            Assert.AreEqual(expectedValue, this.Box.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);

            this.Box.Culture = new CultureInfo(culture2);
            Assert.AreEqual(expectedText, this.Box.Text);
            Assert.AreEqual(expectedFormattedText, this.Box.FormattedText);
            Assert.AreEqual(expectedValue, this.Box.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
        }
    }
}
