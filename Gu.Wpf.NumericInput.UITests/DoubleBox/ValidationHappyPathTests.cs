namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class ValidationHappyPathTests : ValidationTestsBase
    {
        public static readonly Data[] Source =
            {
                new Data("1", "1"),
                new Data(" 1", "1"),
                new Data("1 ", "1"),
                new Data(" 1 ", "1"),
                new Data("1.2", "1.2"),
                new Data("-1.2", "-1.2"),
                new Data("+1.2", "1.2"),
                new Data(".1", "0.1"),
                new Data("-.1", "-0.1"),
                new Data("0.1", "0.1"),
                new Data("1e1", "10"),
                new Data("1e0", "1"),
                new Data("1e-1", "0.1"),
                new Data("1E1", "10"),
                new Data("1E0", "1"),
                new Data("1E-1", "0.1"),
                new Data("-1e1", "-10"),
                new Data("-1e0", "-1"),
                new Data("-1e-1", "-0.1"),
                new Data("-1E1", "-10"),
                new Data("-1E0", "-1"),
                new Data("-1E-1", "-0.1"),
            };

        public static readonly Data[] SwedishSource =
            {
                new Data("1", "1"),
                new Data(" 1", "1"),
                new Data("1 ", "1"),
                new Data(" 1 ", "1"),
                new Data("1,2", "1.2"),
                new Data("-1,2", "-1.2"),
                new Data("+1,2", "1.2"),
                new Data(",1", "0.1"),
                new Data("-,1", "-0.1"),
                new Data("0,1", "0.1"),
                new Data("1e1", "10"),
                new Data("1e0", "1"),
                new Data("1e-1", "0.1"),
                new Data("1E1", "10"),
                new Data("1E0", "1"),
                new Data("1E-1", "0.1"),
                new Data("-1e1", "-10"),
                new Data("-1e0", "-1"),
                new Data("-1e-1", "-0.1"),
                new Data("-1E1", "-10"),
                new Data("-1E0", "-1"),
                new Data("-1E-1", "-0.1"),
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

            this.LoseFocusButton.Click();
        }

        [TestCaseSource(nameof(Source))]
        public void LostFocus(Data data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusBoxValidateOnLostFocusBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(Source))]
        public void LostFocusValidateOnPropertyChanged(Data data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(Source))]
        public void PropertyChanged(Data data)
        {
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(SwedishSource))]
        public void SwedishLostFocus(Data data)
        {
            this.CultureBox.Select("sv-SE");
            var doubleBox = this.Window.Get<TextBox>("LostFocusBoxValidateOnLostFocusBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(SwedishSource))]
        public void SwedishLostFocusValidateOnPropertyChanged(Data data)
        {
            this.CultureBox.Select("sv-SE");
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(SwedishSource))]
        public void SwedishPropertyChanged(Data data)
        {
            this.CultureBox.Select("sv-SE");
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [Test]
        public void WhenNullLostFocus()
        {
            this.CanValueBeNullBox.Checked = true;
            var doubleBox = this.Window.Get<TextBox>("LostFocusBoxValidateOnLostFocusBox");
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
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");
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
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "";
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("", doubleBox.Text);
            Assert.AreEqual("", this.ViewModelValueBox.Text);
        }

        public class Data
        {
            public readonly string Text;
            public readonly string Expected;

            public Data(string text, string expected)
            {
                this.Text = text;
                this.Expected = expected;
            }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Expected}";
        }
    }
}