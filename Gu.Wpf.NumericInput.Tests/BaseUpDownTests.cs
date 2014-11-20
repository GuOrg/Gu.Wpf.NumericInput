namespace Gu.Wpf.NumericInput.Tests
{
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public abstract class BaseUpDownTests
    {
        protected BaseUpDown Box;

        [Test]
        public void IncreaseCommand_CanExecuteChanged_OnReadOnlyChanged()
        {
            var count = 0;
            this.Box.IncreaseCommand.CanExecuteChanged += (sender, args) =>
                { count++; };
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
    }
}
