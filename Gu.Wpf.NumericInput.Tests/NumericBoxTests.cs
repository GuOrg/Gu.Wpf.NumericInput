namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using NUnit.Framework;

    public abstract class NumericBoxTests<T>
        : BaseUpDownTests
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        private readonly Func<NumericBox<T>> _creator;
        private readonly T _max;
        private readonly T _min;
        private readonly T _increment;

        private DummyVm<T> _vm;
        private BindingExpressionBase _bindingExpression;

        protected NumericBoxTests(Func<NumericBox<T>> creator, T min, T max, T increment)
        {
            _creator = creator;
            _min = min;
            _max = max;
            _increment = increment;
        }

        public NumericBox<T> Sut { get { return (NumericBox<T>)Box; } }

        [SetUp]
        public void SetUp()
        {
            Box = _creator();
            Sut.IsReadOnly = false;
            Sut.MinValue = _min;
            Sut.MaxValue = _max;
            Sut.Increment = _increment;
            _vm = new DummyVm<T>();
            var binding = new Binding("Value")
                              {
                                  Source = _vm,
                                  UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                                  Mode = BindingMode.TwoWay
                              };
            _bindingExpression = BindingOperations.SetBinding(Sut, NumericBox<T>.ValueProperty, binding);
            Sut.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

        }
        [TestCase("1", true)]
        [TestCase("1e", false)]
        public void Increase_DecreaseCommand_CanExecute_WithText(string text, bool expected)
        {
            Box.Text = text;
            Assert.AreEqual(expected, Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(expected, Box.IncreaseCommand.CanExecute(null));
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

        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, true)]
        [TestCase(-9, false)]
        [TestCase(-10, false)]
        [TestCase(-11, true)]
        public void SetValueValidates(T value, bool expected)
        {
            Sut.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(Sut));
        }

        [TestCase("9", false)]
        [TestCase("10", false)]
        [TestCase("11", true)]
        [TestCase("-9", false)]
        [TestCase("-10", false)]
        [TestCase("-11", true)]
        [TestCase("1e", true)]
        public void SetTextValidates(string text, bool expected)
        {
            Sut.Text = text;
            Assert.AreEqual(expected, Validation.GetHasError(Sut));
            Assert.AreEqual(text, Sut.Text);
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

        [TestCase("-11", 1)]
        [TestCase("-9", 0)]
        public void DecreaseCommand_CanExecuteChanged_OnTextChanged(string text, int expected)
        {
            var count = 0;
            Sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Sut.Text = text;
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


        [Test]
        public void ValueUpdatesWhenTextIsSet()
        {
            Sut.Text = "1";
            Assert.AreEqual(1, Sut.GetValue(NumericBox<T>.ValueProperty));
        }

        [TestCase(1)]
        public void TextUpdatesWhenValueChanges(T value)
        {
            Sut.SetValue(NumericBox<T>.ValueProperty, value);
            Assert.AreEqual("1", Sut.Text);
        }

        [Test]
        public void AppendDecimalDoesNotTruncateText()
        {
            Sut.Text = "1";
            Assert.AreEqual(1, Sut.GetValue(NumericBox<T>.ValueProperty));
            Assert.AreEqual("1", Sut.Text);
            var floats = new[] { typeof(double), typeof(float), typeof(decimal) };
            if (floats.Contains(typeof(T)))
            {
                Sut.Text = "1.";
                Assert.AreEqual(1, Sut.GetValue(NumericBox<T>.ValueProperty));
                Assert.AreEqual("1.", Sut.Text);

                Sut.Text = "1.0";
                Assert.AreEqual(1, Sut.GetValue(NumericBox<T>.ValueProperty));
                Assert.AreEqual("1.0", Sut.Text);
            }
        }

        [TestCase(1)]
        public void ErrorTextResetsValueFromSource(T expected)
        {
            Sut.Text = "1";
            Assert.AreEqual(expected, Sut.GetValue(NumericBox<T>.ValueProperty));
            Sut.Text = "1e";
            var actual = (T)Sut.GetValue(NumericBox<T>.ValueProperty);
            var hasError = Validation.GetHasError(Box);
            Assert.AreEqual(_vm.Value, actual);
            Assert.IsTrue(hasError);
        }

        [TestCase(2,"1.234",1.234)]
        public void ValueNotAffectedByDecimals(int decimals,string text, T expected)
        {
            Sut.Text = text;
            Sut.Decimals = decimals;
            Assert.AreEqual(expected, Sut.Value);
        }
    }
}