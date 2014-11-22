namespace Gu.Wpf.NumericInput.Tests
{
    using NUnit.Framework;
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
    }
}
