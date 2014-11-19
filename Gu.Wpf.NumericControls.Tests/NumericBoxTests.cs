namespace Gu.Wpf.NumericControls.Tests
{
    using System;
    using NUnit.Framework;
    [RequiresSTA]
    public abstract class NumericBoxTests<T>
        : BaseUpDownTests
        where T : struct, IComparable<T>, IFormattable
    {
        private readonly Func<NumericBox<T>> _creator;
        private readonly T _max;
        private readonly T _min;
        private readonly T _increment;

        protected NumericBoxTests(Func<NumericBox<T>> creator, T min, T max, T increment)
        {
            _creator = creator;
            _min = min;
            _max = max;
            _increment = increment;
        }

        private NumericBox<T> Sut { get { return (NumericBox<T>)Box; } }

        [SetUp]
        public void SetUp()
        {
            Box = _creator();
            Sut.IsReadOnly = false;
            Sut.MinValue = _min;
            Sut.MaxValue = _max;
            Sut.Increment = _increment;
        }

        [Test]
        public void Defaults()
        {
            var box = _creator();
            Assert.AreEqual(1, box.Increment);

            var typeMin = (T)typeof(T).GetField("MinValue").GetValue(null);
            Assert.AreEqual(typeMin, box.MinValue);

            var typeMax = (T)typeof(T).GetField("MaxValue").GetValue(null);
            Assert.AreEqual(typeMax, box.MaxValue);
        }

        [TestCase(9, true)]
        [TestCase(10, false)]
        [TestCase(11, false)]
        public void IncreaseCommand_CanExecute_Value(T value, bool expected)
        {
            Sut.Value = value;
            Assert.AreEqual(expected, Sut.IncreaseCommand.CanExecute(null));
        }

        [TestCase(9, 9)]
        [TestCase(10, 10)]
        [TestCase(11, 10)]
        [TestCase(-9, -9)]
        [TestCase(-10, -10)]
        [TestCase(-11, -10)]
        public void SetValueValidates(T value, T expected)
        {
            Sut.Value = value;
            Assert.AreEqual(expected, Sut.Value);
        }

        [TestCase(8)]
        public void IncreaseCommand_CanExecuteChanged_OnIncrease(T value)
        {
            Sut.Value = value;
            var count = 0;
            Sut.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Assert.IsTrue(Sut.IncreaseCommand.CanExecute(null));
            Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual(0, count);
            Assert.IsTrue(Sut.IncreaseCommand.CanExecute(null));
            Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual(1, count);
            Assert.IsFalse(Sut.IncreaseCommand.CanExecute(null));
        }


        [TestCase(-9, 0)]
        [TestCase(9, 0)]
        [TestCase(11, 1)]
        public void IncreaseCommand_CanExecuteChanged_OnValueChanged(T newValue, int expected)
        {
            var count = 0;
            Sut.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Sut.Value = newValue;
            Assert.AreEqual(expected, count);
        }

        [TestCase(-9, true)]
        [TestCase(-10, false)]
        [TestCase(-11, false)]
        public void DecreaseCommand_CanExecute_Value(T value, bool expected)
        {
            Sut.Value = value;
            Assert.AreEqual(expected, Sut.DecreaseCommand.CanExecute(null));
        }


        [TestCase(-8)]
        public void DecreaseCommand_CanExecute_OnDecrease(T value)
        {
            Sut.Value = value;
            var count = 0;
            Sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Assert.IsTrue(Sut.DecreaseCommand.CanExecute(null));
            Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(0, count);
            Assert.IsTrue(Sut.DecreaseCommand.CanExecute(null));
            Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(1, count);
            Assert.IsFalse(Sut.DecreaseCommand.CanExecute(null));
        }

        [TestCase(-11, 1)]
        [TestCase(-9, 0)]
        public void DecreaseCommand_CanExecuteChanged_OnValueChanged(T newValue, int expected)
        {
            var count = 0;
            Sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Sut.Value = newValue;
            Assert.AreEqual(expected, count);
        }

        [TestCase(0, -1)]
        [TestCase(-9, -10)]
        [TestCase(-10, -10)]
        public void DecreaseCommand_Execute(T value, int expected)
        {
            Sut.Value = value;
            Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(expected, Sut.Value);
        }

        [TestCase(0, 1)]
        [TestCase(9, 10)]
        [TestCase(10, 10)]
        public void IncreaseCommand_Execute(T value, int expected)
        {
            Sut.Value = value;
            Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual(expected, Sut.Value);
        }
    }
}