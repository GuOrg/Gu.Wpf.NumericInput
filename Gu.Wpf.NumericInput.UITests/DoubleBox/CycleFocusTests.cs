namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.WindowsAPI;

    public class CycleFocusTests : WindowTests
    {
        protected override string WindowName { get; } = "CycleFocusWindow";

        [TestCase(true)]
        [TestCase(false)]
        public void WithSpinners(bool withSpinners)
        {
            this.Window.WaitWhileBusy();
            var doubleBoxes = this.Window.GetByText<GroupBox>("DoubleBoxes");
            var textBox = doubleBoxes.Get<TextBox>("TextBox1");
            textBox.Click();
            this.Window.Get<CheckBox>("AllowSpinners").Checked = withSpinners;

            var doubleBox1 = doubleBoxes.Get<TextBox>("DoubleBox1");
            var doubleBox2 = doubleBoxes.Get<TextBox>("DoubleBox2");
            var doubleBox3 = doubleBoxes.Get<TextBox>("DoubleBox3");

            textBox.Click();
            Assert.AreEqual(true, textBox.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            Assert.AreEqual(false, doubleBox3.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(false, textBox.IsFocussed);
            Assert.AreEqual(true, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            Assert.AreEqual(false, doubleBox3.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(false, textBox.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(true, doubleBox2.IsFocussed);
            Assert.AreEqual(false, doubleBox3.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(false, textBox.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            Assert.AreEqual(true, doubleBox3.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(true, textBox.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            Assert.AreEqual(false, doubleBox3.IsFocussed);
        }
    }
}