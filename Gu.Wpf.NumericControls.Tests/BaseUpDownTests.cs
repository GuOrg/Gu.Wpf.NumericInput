namespace Gu.Wpf.NumericControls.Tests
{
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class BaseUpDownTests
    {
        [Test]
        public void IncreaseCommandNotifiesOnReadOnlyChanged()
        {
            var baseUpDownImpl = new BaseUpDownImpl();
            var count = 0;
            baseUpDownImpl.IncreaseCommand.CanExecuteChanged += (sender, args) =>
                { count++; };
            baseUpDownImpl.IsReadOnly = !baseUpDownImpl.IsReadOnly;
            Assert.AreEqual(1, count);
        }

        [Test]
        public void CanIncreaseIfNotReadonly()
        {
            var baseUpDownImpl = new BaseUpDownImpl
                                     {
                                         IsReadOnly = false
                                     };
            Assert.IsTrue(baseUpDownImpl.IncreaseCommand.CanExecute(null));
        }

        [Test]
        public void CanNotIncreaseIfReadonly()
        {
            var baseUpDownImpl = new BaseUpDownImpl
                                     {
                                         IsReadOnly = true
                                     };
            Assert.IsFalse(baseUpDownImpl.IncreaseCommand.CanExecute(null));
        }

        [Test]
        public void DecreaseCommandNotifiesOnReadOnlyChanged()
        {
            var baseUpDownImpl = new BaseUpDownImpl();
            var count = 0;
            baseUpDownImpl.DecreaseCommand.CanExecuteChanged += (sender, args) =>
            { count++; };
            baseUpDownImpl.IsReadOnly = !baseUpDownImpl.IsReadOnly;
            Assert.AreEqual(1, count);
        }


        [Test]
        public void CanDecreaseIfNotReadonly()
        {
            var baseUpDownImpl = new BaseUpDownImpl
            {
                IsReadOnly = false
            };
            Assert.IsTrue(baseUpDownImpl.DecreaseCommand.CanExecute(null));
        }

        [Test]
        public void CanNotDecreaseIfReadonly()
        {
            var baseUpDownImpl = new BaseUpDownImpl
            {
                IsReadOnly = true
            };
            Assert.IsFalse(baseUpDownImpl.DecreaseCommand.CanExecute(null));
        }
    }
}
