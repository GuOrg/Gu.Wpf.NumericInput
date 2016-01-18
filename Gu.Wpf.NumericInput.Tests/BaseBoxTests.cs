namespace Gu.Wpf.NumericInput.Tests
{
    using System.Windows.Controls;

    using NUnit.Framework;

    [TestFixture]
    public abstract class BaseBoxTests
    {
        protected BaseBox Box;

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IncreaseCommand_CanExecute_IsReadonly(bool @readonly, bool expected)
        {
            this.Box.AllowSpinners = true;
            this.Box.Text = "0";
            var count = 0;
            this.Box.IncreaseCommand.CanExecuteChanged += (_, __) => count++;
            this.Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, this.Box.IncreaseCommand.CanExecute(null));
            Assert.AreEqual(@readonly ? 1 : 0, count);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void DecreaseCommand_CanExecute_IsReadonly(bool @readonly, bool expected)
        {
            this.Box.AllowSpinners = true;
            this.Box.Text = "0";
            var count = 0;
            this.Box.DecreaseCommand.CanExecuteChanged += (_, __) => count++;
            this.Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, this.Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(@readonly ? 1 : 0, count);
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

            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
        }
    }
}
