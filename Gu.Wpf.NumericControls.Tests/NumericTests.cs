namespace Gu.Wpf.NumericControls.Tests
{
    using NUnit.Framework;

    [TestFixture]
    [RequiresSTA]
    public class NumericUpDownTests
    {
        [TestCase(1, 2, true)]
        [TestCase(1, 1, false)]
        [TestCase(2, 1, false)]
        public void CanIncreaseTest(double value, double max, bool expected)
        {
            var sut = new NumericUpDownImpl<double>
            {
                MaxValue = max,
                IsReadOnly = false,
                Value = value,
            };
            Assert.AreEqual(expected, sut.IncreaseCommand.CanExecute(null));
        }

        [TestCase(2, 1, 3, 2)]
        [TestCase(-1, 1, 3, 1)]
        [TestCase(4, 1, 3, 3)]
        public void SetValueValidates(double value, double min, double max, double expected)
        {
            var sut = new NumericUpDownImpl<double>
            {
                MinValue = min,
                MaxValue = max,
                IsReadOnly = false,
            };
            sut.Value = value;
            Assert.AreEqual(expected, sut.Value);
        }

        [Test]
        public void UpdatesCanIncreaseOnIncrease()
        {
            Assert.Fail("Check for Value < Max == Max > Max");
        }


        [Test]
        public void UpdatesCanIncreaseOnValueChanged()
        {
            var count = 0;
            var sut = new NumericUpDownImpl<double>
            {
                MinValue = -10,
                MaxValue = 1,
                Increment = 1,
                Value = 0,
                IsReadOnly = false,
            };
            sut.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            sut.Value++;
            Assert.AreEqual(1, count);
        }

        [Test]
        public void UpdatesCanIncreaseOnIncrementChanged()
        {
            Assert.Fail("Check for Value < Max == Max > Max");
        }

        [TestCase(1, 2, false)]
        [TestCase(1, 1, false)]
        [TestCase(2, 1, true)]
        public void CanDecreaseTest(double value, double min, bool expected)
        {
            var sut = new NumericUpDownImpl<double>
            {
                MaxValue = 10,
                Value = value,
                MinValue = min,
                IsReadOnly = false,
            };
            Assert.AreEqual(expected, sut.DecreaseCommand.CanExecute(null));
        }

        [Test]
        public void UpdatesCanDecreaseOnDecrease()
        {
            var count = 0;
            var sut = new NumericUpDownImpl<double>
            {
                MinValue = -10,
                MaxValue = 10,
                Increment = 1,
                Value = 0,
                IsReadOnly = false,
            };
            sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            sut.Value++;
            Assert.AreEqual(1, count);
            sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void UpdatesCanDecreaseOnValueChanged()
        {
            Assert.Fail("Check for Value < Max == Max > Max");
        }

        [Test]
        public void UpdatesCanDecreaseOnIncrementChanged()
        {
            var count = 0;
            var sut = new NumericUpDownImpl<double>
            {
                MinValue = -10,
                MaxValue = 10,
                Increment = 100,
                Value = 0,
                IsReadOnly = false,
            };
            sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Assert.IsFalse(sut.DecreaseCommand.CanExecute(null));
            sut.Increment = 1;
            Assert.AreEqual(1, count);
            Assert.IsTrue(sut.DecreaseCommand.CanExecute(null));
        }

        [TestCase(1, 0, 2, 1)]
        [TestCase(3, 0, 2, 2)]
        [TestCase(-1, 0, 2, 0)]
        public void VaidateValueTest(double value, double min, double max, double expected)
        {
            var sut = new NumericUpDownImpl<double> { MinValue = min, MaxValue = max };
            sut.Value = value;
            Assert.AreEqual(expected, sut.Value);
        }
    }
}