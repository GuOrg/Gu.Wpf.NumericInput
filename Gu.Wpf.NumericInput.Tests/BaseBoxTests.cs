namespace Gu.Wpf.NumericInput.Tests
{
    using System.Windows.Controls;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    [TestFixture]
    public abstract class BaseBoxTests
    {
        protected BaseBox Box;

        [Test]
        public void IncreaseCommand_CanExecuteChanged_OnReadOnlyChanged()
        {
            var count = 0;
            Box.IncreaseCommand.CanExecuteChanged += (sender, args) =>
                { count++; };
            Box.IsReadOnly = !Box.IsReadOnly;
            Assert.AreEqual(1, count);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IncreaseCommand_CanExecute_IsReadonly(bool @readonly, bool expected)
        {
            Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, Box.IncreaseCommand.CanExecute(null));
        }

        [Test]
        public void DecreaseCommand_CanExecuteChanged_OnReadOnlyChanged()
        {
            var count = 0;
            Box.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Box.IsReadOnly = !Box.IsReadOnly;
            Assert.AreEqual(1, count);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void DecreaseCommand_CanExecute_IsReadonly(bool @readonly, bool expected)
        {
            Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, Box.DecreaseCommand.CanExecute(null));
        }

        [TestCase("1", null, false)]
        [TestCase("1", "", false)]
        [TestCase("1", "1", false)]
        [TestCase("1", "2", true)]
        public void PatternValidation(string text, string pattern, bool expected)
        {
            Box.RegexPattern = pattern;
            Box.Text = text;
            Assert.AreEqual(expected, Validation.GetHasError(Box));
        }

        [TestCase("1", null, false, "1", false)]
        [TestCase("1", null, false, "2", true)]
        public void ValidatesOnPatternChanged(string text, string pattern1, bool expected1, string pattern2, bool expected2)
        {
            Box.RegexPattern = pattern1;
            Box.Text = text;
            Assert.AreEqual(expected1, Validation.GetHasError(Box));

            Box.RegexPattern = pattern2;
            Assert.AreEqual(expected2, Validation.GetHasError(Box));
        }
    }
}
