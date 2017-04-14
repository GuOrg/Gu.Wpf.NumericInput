namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using NUnit.Framework;
    using TestStack.White.UIItems;
    using TestStack.White.WindowsAPI;

    public class DefaultCultureWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "DefaultCultureWindow";

        private TextBox ValueTextBox => this.GetCachedTextBox();

        private TextBox SpinnerDoubleBox => this.GetCachedTextBox();

        private TextBox DoubleBox => this.GetCachedTextBox();

        [Test]
        public void OnLoad()
        {
            this.ValueTextBox.BulkText = "1.234";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual("1.234", this.SpinnerDoubleBox.Text);
            Assert.AreEqual("1.234", this.DoubleBox.Text);
        }
    }
}