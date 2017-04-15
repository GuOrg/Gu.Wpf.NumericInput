namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows;
    using Gu.Wpf.NumericInput.Demo;
    using NUnit.Framework;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.Utility;
    using TestStack.White.WindowsAPI;

    [Apartment(ApartmentState.STA)]
    public class MiscTests : WindowTests
    {
        private static readonly IReadOnlyList<string> BoxContainerIds = new[]
        {
            AutomationIds.VanillaGroupBox,
            AutomationIds.DataTemplateGroupBox,
            AutomationIds.ControlTemplate
        };

        protected override string WindowName { get; } = "MiscTestsWindow";

        private TextBox VmValueBox => this.GetCached<TextBox>(AutomationIds.VmValueBox);

        private CheckBox AllowSpinnersBox => this.GetCached<CheckBox>(AutomationIds.AllowSpinnersBox);

        private TextBox DigitsBox => this.GetCached<TextBox>(AutomationIds.DigitsBox);

        private TextBox IncrementBox => this.GetCached<TextBox>(AutomationIds.IncrementBox);

        private TextBox MinBox => this.GetCached<TextBox>(AutomationIds.MinBox);

        private TextBox MaxBox => this.GetCached<TextBox>(AutomationIds.MaxBox);

        [SetUp]
        public void SetUp()
        {
            this.DigitsBox.Enter(string.Empty);
            this.MinBox.Enter(string.Empty);
            this.MaxBox.Enter(string.Empty);
            this.IncrementBox.Enter("1");
            this.VmValueBox.Enter("0");
            this.GetCached<ComboBox>(AutomationIds.CultureBox).Select("en-US");
            this.GetCached<ComboBox>(AutomationIds.ValidationTriggerBox).Select(ValidationTrigger.PropertyChanged.ToString());
            this.GetCached<CheckBox>(AutomationIds.AllowLeadingSignBox).Checked = true;
            this.GetCached<TextBox>(AutomationIds.StringFormatBox).Enter(string.Empty);
            this.GetCached<CheckBox>(AutomationIds.AllowThousandsBox).Checked = false;
            this.AllowSpinnersBox.Checked = false;
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdatesViewModel(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
            inputBox.Enter("1.2");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdatesFromViewModel(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            Assert.AreEqual(this.VmValueBox.Text, inputBox.Text);
            Assert.AreEqual("0", inputBox.Text);
            this.VmValueBox.Enter("1.2");
            inputBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void CanBeNull(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            var canBeNullBox = this.GetCached<CheckBox>(AutomationIds.CanBeNullBox);

            canBeNullBox.Checked = true;
            Assert.AreEqual("0", inputBox.Text);
            inputBox.Enter(string.Empty);
            this.VmValueBox.Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual(string.Empty, this.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Enter("1");
            this.VmValueBox.Click();
            Assert.AreEqual("1", inputBox.EditText());
            Assert.AreEqual("1", inputBox.FormattedText());
            Assert.AreEqual("1", this.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            canBeNullBox.Checked = false;
            inputBox.Enter(string.Empty);
            this.VmValueBox.Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual("1", this.VmValueBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            canBeNullBox.Checked = true;
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual("1", this.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Click();
            this.VmValueBox.Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual(string.Empty, this.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Culture(string containerId)
        {
            this.GetCached<CheckBox>(AutomationIds.AllowThousandsBox).Checked = false;
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            var cultureBox = this.GetCached<ComboBox>(AutomationIds.CultureBox);
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());
            inputBox.Enter("1.2");
            Assert.AreEqual(false, inputBox.HasValidationError());
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());

            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            cultureBox.Select("sv-SE");
            this.VmValueBox.Click();
            Assert.AreEqual("1,2", inputBox.EditText());
            Assert.AreEqual("1,2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            cultureBox.Select("en-US");
            this.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Click();
            this.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2.3", this.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Enter("5.6a");
            this.VmValueBox.Click();
            Assert.AreEqual("5.6a", inputBox.EditText());
            Assert.AreEqual("5.6a", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("2.3", this.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            cultureBox.Select("sv-SE");
            this.VmValueBox.Click();
            Assert.AreEqual("5.6a", inputBox.EditText());
            Assert.AreEqual("5.6a", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("2.3", this.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void ValidationTriggerLostFocus(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            this.GetCached<ComboBox>(AutomationIds.ValidationTriggerBox).Select(ValidationTrigger.LostFocus.ToString());
            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
            inputBox.Enter("1.2");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Enter("ggg");
            Assert.AreEqual("ggg", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            this.VmValueBox.Click();
            Assert.AreEqual("ggg", inputBox.EditText());
            Assert.AreEqual("ggg", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void NumberStylesAllowLeadingSign(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            var signBox = this.GetCached<CheckBox>(AutomationIds.AllowLeadingSignBox);
            inputBox.Enter("-1.2");
            this.VmValueBox.Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", this.VmValueBox.Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            signBox.Checked = false;
            this.VmValueBox.Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", this.VmValueBox.Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            signBox.Checked = true;
            this.VmValueBox.Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", this.VmValueBox.Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdateDigitsWhenGreaterThanMax(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            var digitsBox = this.DigitsBox;
            var maxBox = this.MaxBox;
            Assert.AreEqual("0", inputBox.Text);
            inputBox.Enter("1.2");
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("0", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            maxBox.Enter("1");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            digitsBox.Enter("4");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2000", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void DecimalDigits(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            var digitsBox = this.DigitsBox;
            Assert.AreEqual("0", inputBox.Text);
            digitsBox.Enter("4");
            this.VmValueBox.Click();
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0.0000", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("0", this.VmValueBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            this.VmValueBox.Enter("1.2");
            inputBox.Click();
            Assert.AreEqual("1.2", inputBox.Text);
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2000", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            digitsBox.Enter("0");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            digitsBox.Enter("-3");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Enter("1.234567");
            this.VmValueBox.Click();
            Assert.AreEqual("1.234567", inputBox.EditText());
            Assert.AreEqual("1.235", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.234567", this.VmValueBox.Text);
            Assert.AreEqual("1.234567", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            digitsBox.Enter("4");
            this.VmValueBox.Click();
            Assert.AreEqual("1.234567", inputBox.EditText());
            Assert.AreEqual("1.2346", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.234567", this.VmValueBox.Text);
            Assert.AreEqual("1.234567", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void StringFormat(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            var stringFormatBox = this.GetCached<TextBox>(AutomationIds.StringFormatBox);

            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());

            inputBox.Enter("123456.78");
            this.VmValueBox.Click();
            Assert.AreEqual("123456.78", inputBox.Text);
            Assert.AreEqual("123456.78", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("123456.78", this.VmValueBox.Text);
            Assert.AreEqual("123456.78", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            this.GetCached<CheckBox>(AutomationIds.AllowThousandsBox).Checked = true;
            stringFormatBox.Enter("N3");
            this.VmValueBox.Click();
            Assert.AreEqual("123456.78", inputBox.EditText());
            Assert.AreEqual("123,456.780", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("123456.78", this.VmValueBox.Text);
            Assert.AreEqual("123456.78", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Enter("2222.33333");
            this.VmValueBox.Click();
            Assert.AreEqual("2222.33333", inputBox.EditText());
            Assert.AreEqual("2,222.333", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2222.33333", this.VmValueBox.Text);
            Assert.AreEqual("2222.33333", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            this.VmValueBox.Enter("4444.5555");
            inputBox.Click();
            Assert.AreEqual("4444.5555", inputBox.Text);
            Assert.AreEqual("4,444.556", inputBox.FormattedText());

            this.VmValueBox.Click();
            Assert.AreEqual("4444.5555", inputBox.EditText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("4444.5555", this.VmValueBox.Text);
            Assert.AreEqual("4444.5555", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Max(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            var maxBox = this.MaxBox;
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());
            inputBox.Enter("1.2");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            maxBox.Enter("-1");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());

            this.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            maxBox.Enter("6");
            this.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            inputBox.Click();
            this.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2.3", this.VmValueBox.Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            this.VmValueBox.Enter("7.8");
            inputBox.Click();
            Assert.AreEqual("7.8", inputBox.Text);
            Assert.AreEqual("7.8", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("7.8", this.VmValueBox.Text);
            Assert.AreEqual("7.8", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());

            maxBox.Enter("10");
            this.VmValueBox.Click();
            Assert.AreEqual("7.8", inputBox.Text);
            Assert.AreEqual("7.8", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("7.8", this.VmValueBox.Text);
            Assert.AreEqual("7.8", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual(Status.Idle, inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Min(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            var minBox = this.MinBox;
            Assert.AreNotEqual("1.2", inputBox.Text);
            inputBox.Enter("1.2");
            this.VmValueBox.Click();
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual("1.2", this.VmValueBox.Text);

            minBox.Enter("4");
            this.VmValueBox.Click();
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual("1.2", this.VmValueBox.Text);

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text);

            minBox.Enter("-2");
            this.VmValueBox.Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", this.VmValueBox.Text); // maybe we want to update source here idk.

            inputBox.Enter("5.6");
            this.VmValueBox.Click();
            Assert.AreEqual("5.6", inputBox.Text);
            Assert.AreEqual("5.6", this.VmValueBox.Text);
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Undo(string containerId)
        {
            var container = this.GetCached<UIItemContainer>(containerId);
            var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
            Assert.AreEqual("0", inputBox.Text);
            var keyboard = this.Window.Keyboard;
            inputBox.Click();
            keyboard.Enter("1");
            Assert.AreEqual("10", inputBox.Text);
            keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            keyboard.Enter("z");
            keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", this.VmValueBox.Text);
        }

        [Test]
        public void CopyTest()
        {
            var inputBox = this.Window.Get<TextBox>(AutomationIds.InputBox);
            var attachedKeyboard = this.Window.Keyboard;
            inputBox.Text = "1.2";
            attachedKeyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            attachedKeyboard.Enter("ac");
            attachedKeyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);

            // check the text is the same as that on the clipboard
            Retry.For(
                () =>
                {
                    var clipboardText = Clipboard.GetText();
                    Assert.AreEqual("1.2", clipboardText);
                },
                TimeSpan.FromSeconds(5));
        }
    }
}