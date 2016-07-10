namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class ValidationErrorMinMaxTests : ValidationTestsBase
    {
        public static readonly MinMaxData[] MinMaxSource =
            {
                new MinMaxData("-2", "-1", "", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1'"),
                new MinMaxData("-2.1", "-1.1", "", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.1'"),
                new MinMaxData("-2", "-1", "1", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1'"),
                new MinMaxData("-2.1", "-1.1", "1.1", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.1'"),
                new MinMaxData("2", "", "1", "1", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1'"),
                new MinMaxData("2.1", "", "1.1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.1'"),
                new MinMaxData("2", "-1", "1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1'"),
                new MinMaxData("2.1", "-1.1", "1.1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.1'"),
            };

        [SetUp]
        public void SetUp()
        {
            this.ViewModelValueBox.Text = "0";
            this.CultureBox.Select("en-US");
            this.CanValueBeNullBox.Checked = false;

            this.AllowLeadingWhiteBox.Checked = true;
            this.AllowTrailingWhiteBox.Checked = true;
            this.AllowLeadingSignBox.Checked = true;
            this.AllowDecimalPointBox.Checked = true;
            this.AllowThousandsBox.Checked = false;
            this.AllowExponentBox.Checked = true;

            this.MinBox.Text = "";
            this.MaxBox.Text = "";
            this.LoseFocusButton.Click();
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxLostFocus(MinMaxData data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = data.StartValue;
            this.LoseFocusButton.Click();
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;

            doubleBox.BulkText = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxLostFocusValidateOnPropertyChanged(MinMaxData data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = data.StartValue;
            this.LoseFocusButton.Click();
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.BulkText = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxPropertyChanged(MinMaxData data)
        {
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = data.StartValue;
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.BulkText = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxPropertyChangedWhenNull(MinMaxData data)
        {
            this.CanValueBeNullBox.Checked = true;
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "";
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("", this.ViewModelValueBox.Text);
        }

        public class MinMaxData
        {
            public readonly string Text;
            public readonly string Min;
            public readonly string Max;
            public readonly string Expected;
            public readonly string ExpectedMessage;

            public MinMaxData(string text, string min, string max, string expected, string expectedMessage)
            {
                this.Text = text;
                this.Min = min;
                this.Max = max;
                this.Expected = expected;
                this.ExpectedMessage = expectedMessage;
            }

            public string StartValue => string.IsNullOrEmpty(this.Min)
                                            ? this.Max
                                            : this.Min;

            public override string ToString() => $"Text: {this.Text}, Min: {this.Min}, Max: {this.Max}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedMessage}";
        }
    }
}