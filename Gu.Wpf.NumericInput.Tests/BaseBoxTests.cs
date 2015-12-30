namespace Gu.Wpf.NumericInput.Tests
{
    using System.Windows.Controls;

    using NUnit.Framework;

    [TestFixture]
    public abstract class BaseBoxTests
    {
        protected BaseBox Box;

        [Test]
        public void IncreaseCommand_CanExecuteChanged_OnReadOnlyChanged()
        {
            var count = 0;
            this.Box.AllowSpinners = true;
            this.Box.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            this.Box.IsReadOnly = !this.Box.IsReadOnly;
            Assert.AreEqual(1, count);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IncreaseCommand_CanExecute_IsReadonly(bool @readonly, bool expected)
        {
            this.Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, this.Box.IncreaseCommand.CanExecute(null));
        }

        [Test]
        public void DecreaseCommand_CanExecuteChanged_OnReadOnlyChanged()
        {
            var count = 0;
            this.Box.AllowSpinners = true;
            this.Box.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            this.Box.IsReadOnly = !this.Box.IsReadOnly;
            Assert.AreEqual(1, count);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void DecreaseCommand_CanExecute_IsReadonly(bool @readonly, bool expected)
        {
            this.Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, this.Box.DecreaseCommand.CanExecute(null));
        }

        [TestCase("1", null, false)]
        [TestCase("1", "", false)]
        [TestCase("1", "1", false)]
        [TestCase("1", "2", true)]
        public void PatternValidation(string text, string pattern, bool expected)
        {
            this.Box.RegexPattern = pattern;
            this.Box.Text = text;
            Assert.AreEqual(expected, Validation.GetHasError(this.Box));
        }

        [TestCase("1", null, false, "1", false)]
        [TestCase("1", null, false, "2", true)]
        public void ValidatesOnPatternChanged(string text, string pattern1, bool expected1, string pattern2, bool expected2)
        {
            this.Box.RegexPattern = pattern1;
            this.Box.Text = text;
            Assert.AreEqual(expected1, Validation.GetHasError(this.Box));

            this.Box.RegexPattern = pattern2;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Box));
        }
    }
}
