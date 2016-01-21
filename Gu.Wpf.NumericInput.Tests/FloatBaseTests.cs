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
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
            Assert.AreEqual(expected, this.Sut.Text);
            this.Sut.RaiseLostFocus();
            Assert.AreEqual(expected, this.Sut.FormattedText);
        }

        [TestCase(2, "1.234", "1.23", "1.234")]
        public void ValueNotAffectedByDecimalDigits(int decimals, string text, string expectedText, string expectedValue)
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 3);
            this.Sut.Text = text;
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals);
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
            Assert.AreEqual(text, this.Sut.Text);
            this.Sut.RaiseLostFocus();
            Assert.AreEqual(expectedText, this.Sut.FormattedText);
            Assert.AreEqual(expectedValue, this.Sut.Value.ToString());
        }

        [TestCase(2, 3, "1.234", "1.23", "1.234")]
        [TestCase(3, 2, "1.234", "1.234", "1.23")]
        public void DecimalDigitsWhenValueFromDataContext(int decimals1, int decimals2, string text, string expectedText1, string expectedText2)
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals1);
            var value = this.Sut.Parse(text);
            var statuses = new List<Status>();
            var expectedStatuses = new List<Status>();
            var sources = new List<TextSource>();
            using (this.Sut.PropertyChanged(BaseBox.StatusProperty, x => statuses.Add((Status)x.NewValue)))
            using (this.Sut.PropertyChanged(BaseBox.TextSourceProperty, x => sources.Add((TextSource)x.NewValue)))
            {
                this.Vm.Value = value;
                Assert.AreEqual(text, this.Sut.Text);
                Assert.AreEqual(expectedText1, this.Sut.FormattedText);
                Assert.AreEqual(value, this.Sut.Value);
                expectedStatuses.AddRange(new[] { Status.UpdatingFromValueBinding, Status.Validating, Status.Idle });
                CollectionAssert.AreEqual(expectedStatuses, statuses);
                CollectionAssert.AreEqual(new[] { TextSource.ValueBinding }, sources);

                this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals2);
                Assert.AreEqual(Status.Idle, this.Sut.Status);
                Assert.AreEqual(TextSource.ValueBinding, this.Sut.TextSource);
                Assert.AreEqual(text, this.Sut.Text);
                Assert.AreEqual(expectedText2, this.Sut.FormattedText);
                Assert.AreEqual(value, this.Sut.Value);
                expectedStatuses.AddRange(new[] { Status.Validating, Status.Idle,  });
                CollectionAssert.AreEqual(expectedStatuses, statuses);
                CollectionAssert.AreEqual(new[] { TextSource.ValueBinding }, sources);

                this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals1);
                Assert.AreEqual(text, this.Sut.Text);
                Assert.AreEqual(expectedText1, this.Sut.FormattedText);
                Assert.AreEqual(value, this.Sut.Value);
                CollectionAssert.AreEqual(new[] { TextSource.ValueBinding }, sources);
            }
        }

        [TestCase(2, 1, "1.234", "1.23")]
        public void DigitsUpdatesWhenGreaterThanMax(int decimals, T max, string text, string expectedText)
        {
            this.Sut.Text = text;
            this.Sut.MaxValue = max;
            Assert.AreEqual(true, Validation.GetHasError(this.Sut));

            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals);
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
            Assert.AreEqual(text, this.Sut.Text);
            Assert.AreEqual(expectedText, this.Sut.FormattedText);
            Assert.AreEqual(null, this.Sut.Value);
            Assert.AreEqual(true, Validation.GetHasError(this.Sut));
        }

        [TestCase("1.234", "1.234", "1.23", "1.23")]
        public void ValueUpdatedOnFewerDecimalDigitsFromUser(string text1, string expected1, string text2, string expected2)
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 5);
            this.Sut.Text = text1;
            var actual = this.Sut.Value.ToString();
            Assert.AreEqual(expected1, actual);
            this.Sut.Text = text2;
            var actual2 = this.Sut.Value.ToString();
            Assert.AreEqual(expected2, actual2);
        }

        [TestCase("1.234", 2, "1.23", 4, "1.2340")]
        public void RoundtripDecimals(string text, int decimals1, string expected1, int decimals2, string expected2)
        {
            this.Sut.Text = text;
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals1);
            Assert.AreEqual(text, this.Sut.Text);
            Assert.AreEqual(expected1, this.Sut.FormattedText);
            Assert.AreEqual(text, this.Sut.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);

            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, decimals2);
            Assert.AreEqual(text, this.Sut.Text);
            Assert.AreEqual(expected2, this.Sut.FormattedText);
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
        }

        [Test]
        public void AddedDigitsNotTruncated()
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 2);
            this.Sut.Text = "1.23";
            this.Sut.Text = "1.234";
            Assert.AreEqual("1.234", this.Sut.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
        }

        [Test]
        public void FewerDecimalsUpdatesValue()
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 4);
            this.Sut.Text = "1.2334";
            this.Sut.Text = "1.23";
            Assert.AreEqual("1.23", this.Sut.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
        }

        [TestCase("sv-SE", "1,23", "en-US", "1.23", "1.2", "1.23")]
        [TestCase("en-US", "1.23", "sv-SE", "1,23", "1,2", "1.23")]
        [TestCase("en-US", "1.23e", "sv-SE", "1.23e", "1.23e", "")]
        public void ChangeCultureDoesNotTruncateDecimals(string culture1, string text, string culture2, string expectedText, string expectedFormattedText, string expectedValue)
        {
            this.Sut.SetValue(DecimalDigitsBox<T>.DecimalDigitsProperty, 1);
            this.Sut.Culture = new CultureInfo(culture1);
            this.Sut.Text = text;
            Assert.AreEqual(text, this.Sut.Text);
            this.Sut.UpdateFormat();
            //Assert.AreEqual(text, this.Sut.FormattedText);
            Assert.AreEqual(expectedValue, this.Sut.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);

            this.Sut.Culture = new CultureInfo(culture2);
            Assert.AreEqual(expectedText, this.Sut.Text);
            Assert.AreEqual(expectedFormattedText, this.Sut.FormattedText);
            Assert.AreEqual(expectedValue, this.Sut.Value.ToString());
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
        }
    }
}
