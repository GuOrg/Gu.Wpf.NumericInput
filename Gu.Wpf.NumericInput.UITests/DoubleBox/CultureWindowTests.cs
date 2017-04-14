namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using NUnit.Framework;
    using TestStack.White.UIItems;
    using TestStack.White.WindowsAPI;

    public class CultureWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "CultureWindow";

        private TextBox ValueTextBox => this.GetCachedTextBox();

        private TextBox SpinnerDoubleBox => this.GetCachedTextBox();

        private TextBox InheritingCultureDoubleBox => this.GetCachedTextBox();

        private TextBox SvSeDoubleBox => this.GetCachedTextBox();

        private TextBox EnUsDoubleBox => this.GetCachedTextBox();

        private TextBox BoundCultureDoubleBox => this.GetCachedTextBox();

        private TextBox CultureTextBox => this.GetCachedTextBox();

        [Test]
        public void TestCultures()
        {
            this.ValueTextBox.BulkText = "1.234";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual("1,234", this.SpinnerDoubleBox.Text);
            Assert.AreEqual("1,234", this.InheritingCultureDoubleBox.Text);
            Assert.AreEqual("1,234", this.SvSeDoubleBox.Text);
            Assert.AreEqual("1.234", this.EnUsDoubleBox.Text);
            Assert.AreEqual("1,234", this.BoundCultureDoubleBox.Text);

            this.CultureTextBox.Text = "en-us";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual("1,234", this.SpinnerDoubleBox.Text);
            Assert.AreEqual("1,234", this.InheritingCultureDoubleBox.Text);
            Assert.AreEqual("1,234", this.SvSeDoubleBox.Text);
            Assert.AreEqual("1.234", this.EnUsDoubleBox.Text);
            Assert.AreEqual("1.234", this.BoundCultureDoubleBox.Text);
        }
    }
}