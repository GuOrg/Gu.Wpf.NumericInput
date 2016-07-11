// ReSharper disable MemberCanBePrivate.Global
namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using NUnit.Framework;

    public class ValidationHappyPathTests : DoubleBoxTestsBase
    {
        public static readonly ValidationData[] Source =
            {
                new ValidationData("1", "1"),
                new ValidationData(" 1", "1"),
                new ValidationData("1 ", "1"),
                new ValidationData(" 1 ", "1"),
                new ValidationData("1.2", "1.2"),
                new ValidationData("-1.2", "-1.2"),
                new ValidationData("+1.2", "1.2"),
                new ValidationData(".1", "0.1"),
                new ValidationData("-.1", "-0.1"),
                new ValidationData("0.1", "0.1"),
                new ValidationData("1e1", "10"),
                new ValidationData("1e0", "1"),
                new ValidationData("1e-1", "0.1"),
                new ValidationData("1E1", "10"),
                new ValidationData("1E0", "1"),
                new ValidationData("1E-1", "0.1"),
                new ValidationData("-1e1", "-10"),
                new ValidationData("-1e0", "-1"),
                new ValidationData("-1e-1", "-0.1"),
                new ValidationData("-1E1", "-10"),
                new ValidationData("-1E0", "-1"),
                new ValidationData("-1E-1", "-0.1"),
            };

        public static readonly ValidationData[] SwedishSource =
            {
                new ValidationData("1", "1"),
                new ValidationData(" 1", "1"),
                new ValidationData("1 ", "1"),
                new ValidationData(" 1 ", "1"),
                new ValidationData("1,2", "1.2"),
                new ValidationData("-1,2", "-1.2"),
                new ValidationData("+1,2", "1.2"),
                new ValidationData(",1", "0.1"),
                new ValidationData("-,1", "-0.1"),
                new ValidationData("0,1", "0.1"),
                new ValidationData("1e1", "10"),
                new ValidationData("1e0", "1"),
                new ValidationData("1e-1", "0.1"),
                new ValidationData("1E1", "10"),
                new ValidationData("1E0", "1"),
                new ValidationData("1E-1", "0.1"),
                new ValidationData("-1e1", "-10"),
                new ValidationData("-1e0", "-1"),
                new ValidationData("-1e-1", "-0.1"),
                new ValidationData("-1E1", "-10"),
                new ValidationData("-1E0", "-1"),
                new ValidationData("-1E-1", "-0.1"),
            };

        public static readonly MinMaxData[] MinMaxSource =
            {
                new MinMaxData("1", "", "", "1"),
                new MinMaxData("-1", "-1", "", "-1"),
                new MinMaxData("-1", "-10", "", "-1"),
                new MinMaxData("1", "", "1", "1"),
                new MinMaxData("1", "", "10", "1"),
                new MinMaxData("-2", "-2", "2", "-2"),
                new MinMaxData("-1", "-2", "2", "-1"),
                new MinMaxData("1", "-2", "2", "1"),
                new MinMaxData("2", "-2", "2", "2"),
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

        [TestCaseSource(nameof(Source))]
        public void LostFocusValidateOnLostFocus(ValidationData validationData)
        {
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = validationData.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual(validationData.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(Source))]
        public void LostFocusValidateOnPropertyChanged(ValidationData validationData)
        {
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = validationData.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual(validationData.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(Source))]
        public void PropertyChanged(ValidationData validationData)
        {
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = validationData.Text;
            this.Window.WaitWhileBusy();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual(validationData.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishSource))]
        public void SwedishLostFocusValidateOnLostFocus(ValidationData validationData)
        {
            this.CultureBox.Select("sv-SE");
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = validationData.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual(validationData.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishSource))]
        public void SwedishLostFocusValidateOnPropertyChanged(ValidationData validationData)
        {
            this.CultureBox.Select("sv-SE");
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = validationData.Text;
            this.Window.WaitWhileBusy();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual(validationData.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(SwedishSource))]
        public void SwedishPropertyChangedValidateOnPropertyChanged(ValidationData validationData)
        {
            this.CultureBox.Select("sv-SE");
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = validationData.Text;
            this.Window.WaitWhileBusy();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(validationData.Text, doubleBox.Text);
            Assert.AreEqual(validationData.Expected, this.ViewModelValueBox.Text);
        }

        [Test]
        public void WhenNullLostFocusValidateOnLostFocus()
        {
            this.CanValueBeNullBox.Checked = true;
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = "";
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("", doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("", doubleBox.Text);
            Assert.AreEqual("", this.ViewModelValueBox.Text);
        }

        [Test]
        public void WhenNullLostFocusValidateOnPropertyChanged()
        {
            this.CanValueBeNullBox.Checked = true;
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = "";
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("", doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("", doubleBox.Text);
            Assert.AreEqual("", this.ViewModelValueBox.Text);
        }

        [Test]
        public void WheNullPropertyChanged()
        {
            this.CanValueBeNullBox.Checked = true;
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = "";
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("", doubleBox.Text);
            Assert.AreEqual("", this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxLostFocus(MinMaxData data)
        {
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxLostFocusValidateOnPropertyChanged(MinMaxData data)
        {
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxPropertyChanged(MinMaxData data)
        {
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        public class ValidationData
        {
            public readonly string Text;
            public readonly string Expected;

            public ValidationData(string text, string expected)
            {
                this.Text = text;
                this.Expected = expected;
            }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Expected}";
        }

        public class MinMaxData
        {
            public readonly string Text;
            public readonly string Min;
            public readonly string Max;
            public readonly string Expected;

            public MinMaxData(string text, string min, string max, string expected)
            {
                this.Text = text;
                this.Min = min;
                this.Max = max;
                this.Expected = expected;
            }

            public override string ToString() => $"Text: {this.Text}, Min: {this.Min}, Max: {this.Max}, Expected: {this.Expected}";
        }
    }
}