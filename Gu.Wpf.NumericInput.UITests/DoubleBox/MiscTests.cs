namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using FlaUI.Core.Definitions;
    using FlaUI.Core.Input;
    using FlaUI.Core.WindowsAPI;
    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    public sealed class MiscTests : IDisposable
    {
        private static readonly IReadOnlyList<string> BoxContainerIds = new[]
        {
            "VanillaGroupBox",
            "DataTemplateGroupBox",
            "ControlTemplate"
        };

        private readonly DoubleBoxView view;

        private bool disposed;

        public MiscTests()
        {
            this.view = new DoubleBoxView("MiscTestsWindow");
        }

        [SetUp]
        public void SetUp()
        {
            this.view.Reset();
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdatesViewModel(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
            inputBox.Enter("1.2");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdatesFromViewModel(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            Assert.AreEqual(this.view.VmValueBox.Text, inputBox.Text);
            Assert.AreEqual("0", inputBox.Text);
            this.view.VmValueBox.Enter("1.2");
            inputBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void CanBeNull(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var canBeNullBox = this.view.Window.FindCheckBox("CanBeNullBox");

            canBeNullBox.State = ToggleState.On;
            Assert.AreEqual("0", inputBox.Text);
            inputBox.Enter(string.Empty);
            this.view.VmValueBox.Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual(string.Empty, this.view.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("1");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1", inputBox.EditText());
            Assert.AreEqual("1", inputBox.FormattedText());
            Assert.AreEqual("1", this.view.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            canBeNullBox.State = ToggleState.Off;
            inputBox.Enter(string.Empty);
            this.view.VmValueBox.Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual("1", this.view.VmValueBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            canBeNullBox.State = ToggleState.On;
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual("1", this.view.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Click();
            this.view.VmValueBox.Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual(string.Empty, this.view.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Culture(string containerId)
        {
            this.view.Window.FindCheckBox("AllowThousandsBox").State = ToggleState.Off;
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var cultureBox = this.view.Window.FindComboBox("CultureBox");
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());
            inputBox.Enter("1.2");
            Assert.AreEqual(false, inputBox.HasValidationError());
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());

            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            cultureBox.Select("sv-SE");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1,2", inputBox.EditText());
            Assert.AreEqual("1,2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            cultureBox.Select("en-US");
            this.view.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Click();
            this.view.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2.3", this.view.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("5.6a");
            this.view.VmValueBox.Click();
            Assert.AreEqual("5.6a", inputBox.EditText());
            Assert.AreEqual("5.6a", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("2.3", this.view.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            cultureBox.Select("sv-SE");
            this.view.VmValueBox.Click();
            Assert.AreEqual("5.6a", inputBox.EditText());
            Assert.AreEqual("5.6a", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("2.3", this.view.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void ValidationTriggerLostFocus(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            this.view.Window.FindComboBox("ValidationTriggerBox").Select(ValidationTrigger.LostFocus.ToString());
            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
            inputBox.Enter("1.2");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("ggg");
            Assert.AreEqual("ggg", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            this.view.VmValueBox.Click();
            Assert.AreEqual("ggg", inputBox.EditText());
            Assert.AreEqual("ggg", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void NumberStylesAllowLeadingSign(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var signBox = this.view.Window.FindCheckBox("AllowLeadingSignBox");
            inputBox.Enter("-1.2");
            this.view.VmValueBox.Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            signBox.State = ToggleState.Off;
            this.view.VmValueBox.Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            signBox.State = ToggleState.On;
            this.view.VmValueBox.Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdateDigitsWhenGreaterThanMax(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var digitsBox = this.view.DigitsBox;
            var maxBox = this.view.MaxBox;
            Assert.AreEqual("0", inputBox.Text);
            inputBox.Enter("1.2");
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("0", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            maxBox.Enter("1");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            digitsBox.Enter("4");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2000", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void DecimalDigits(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var digitsBox = this.view.DigitsBox;
            Assert.AreEqual("0", inputBox.Text);
            digitsBox.Enter("4");
            this.view.VmValueBox.Click();
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0.0000", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("0", this.view.VmValueBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            this.view.VmValueBox.Enter("1.2");
            inputBox.Click();
            Assert.AreEqual("1.2", inputBox.Text);
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2000", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            digitsBox.Enter("0");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            digitsBox.Enter("-3");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("1.234567");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.234567", inputBox.EditText());
            Assert.AreEqual("1.235", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.234567", this.view.VmValueBox.Text);
            Assert.AreEqual("1.234567", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            digitsBox.Enter("4");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.234567", inputBox.EditText());
            Assert.AreEqual("1.2346", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.234567", this.view.VmValueBox.Text);
            Assert.AreEqual("1.234567", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void StringFormat(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var stringFormatBox = this.view.Window.FindTextBox("StringFormatBox");

            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());

            inputBox.Enter("123456.78");
            this.view.VmValueBox.Click();
            Assert.AreEqual("123456.78", inputBox.Text);
            Assert.AreEqual("123456.78", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("123456.78", this.view.VmValueBox.Text);
            Assert.AreEqual("123456.78", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            this.view.Window.FindCheckBox("AllowThousandsBox").State = ToggleState.On;
            stringFormatBox.Enter("N3");
            this.view.VmValueBox.Click();
            Assert.AreEqual("123456.78", inputBox.EditText());
            Assert.AreEqual("123,456.780", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("123456.78", this.view.VmValueBox.Text);
            Assert.AreEqual("123456.78", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("2222.33333");
            this.view.VmValueBox.Click();
            Assert.AreEqual("2222.33333", inputBox.EditText());
            Assert.AreEqual("2,222.333", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2222.33333", this.view.VmValueBox.Text);
            Assert.AreEqual("2222.33333", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            this.view.VmValueBox.Enter("4444.5555");
            inputBox.Click();
            Assert.AreEqual("4444.5555", inputBox.Text);
            Assert.AreEqual("4,444.556", inputBox.FormattedText());

            this.view.VmValueBox.Click();
            Assert.AreEqual("4444.5555", inputBox.EditText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("4444.5555", this.view.VmValueBox.Text);
            Assert.AreEqual("4444.5555", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Max(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var maxBox = this.view.MaxBox;
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());
            inputBox.Enter("1.2");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            maxBox.Enter("-1");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());

            this.view.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            maxBox.Enter("6");
            this.view.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Click();
            this.view.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2.3", this.view.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            this.view.VmValueBox.Enter("7.8");
            inputBox.Click();
            Assert.AreEqual("7.8", inputBox.Text);
            Assert.AreEqual("7.8", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("7.8", this.view.VmValueBox.Text);
            Assert.AreEqual("7.8", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            maxBox.Enter("10");
            this.view.VmValueBox.Click();
            Assert.AreEqual("7.8", inputBox.Text);
            Assert.AreEqual("7.8", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("7.8", this.view.VmValueBox.Text);
            Assert.AreEqual("7.8", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Min(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var minBox = this.view.MinBox;
            Assert.AreNotEqual("1.2", inputBox.Text);
            inputBox.Enter("1.2");
            this.view.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);

            minBox.Enter("4");
            this.view.VmValueBox.Click();
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text);

            minBox.Enter("-2");
            this.view.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.view.VmValueBox.Text); // maybe we want to update source here idk.

            inputBox.Enter("5.6");
            this.view.VmValueBox.Click();
            Assert.AreEqual("5.6", inputBox.Text);
            Assert.AreEqual("5.6", this.view.VmValueBox.Text);
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Undo(string containerId)
        {
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            Assert.AreEqual("0", inputBox.Text);
            inputBox.Click();
            Keyboard.Type("1");
            Assert.AreEqual("10", inputBox.Text);
            using (Keyboard.Pressing(VirtualKeyShort.CONTROL))
            {
                Keyboard.Type("z");
            }

            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", this.view.VmValueBox.Text);
        }

        [Test]
        public void CopyTest()
        {
            var inputBox = this.view.Window.FindTextBox("InputBox");
            inputBox.Text = "1.2";
            using (Keyboard.Pressing(VirtualKeyShort.CONTROL))
            {
                Keyboard.Type("ac");
            }

            Thread.Sleep(100);
            var clipboardText = System.Windows.Clipboard.GetText();
            Assert.AreEqual("1.2", clipboardText);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.view.Dispose();
        }
    }
}