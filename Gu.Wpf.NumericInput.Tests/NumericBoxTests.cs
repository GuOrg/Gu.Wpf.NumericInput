namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using NUnit.Framework;

    [RequiresSTA]
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
            this._creator = creator;
            this._min = min;
            this._max = max;
            this._increment = increment;
        }

        public NumericBox<T> Sut { get { return (NumericBox<T>)this.Box; } }

        [SetUp]
        public void SetUp()
        {
            this.Box = this._creator();
            this.Sut.IsReadOnly = false;
            this.Sut.MinValue = this._min;
            this.Sut.MaxValue = this._max;
            this.Sut.Increment = this._increment;
            this._vm = new DummyVm<T>();
            var binding = new Binding("Value")
                              {
                                  Source = this._vm,
                                  UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                                  Mode = BindingMode.TwoWay
                              };
            this._bindingExpression = BindingOperations.SetBinding(this.Sut, NumericBox<T>.ValueProperty, binding);
            this.Sut.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

        }
        [TestCase("1", true)]
        [TestCase("1e", false)]
        public void Increase_DecreaseCommand_CanExecute_WithText(string text, bool expected)
        {
            this.Box.Text = text;
            Assert.AreEqual(expected, this.Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(expected, this.Box.IncreaseCommand.CanExecute(null));
        }

        [Test]
        public void Defaults()
        {
            var box = this._creator();
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
            this.Sut.Value = value;
            Assert.AreEqual(expected, this.Sut.IncreaseCommand.CanExecute(null));
        }

        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, true)]
        [TestCase(-9, false)]
        [TestCase(-10, false)]
        [TestCase(-11, true)]
        public void SetValueValidates(T value, bool expected)
        {
            this.Sut.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Sut));
        }

        [TestCase("9", false)]
        [TestCase("10", false)]
        [TestCase("11", true)]
        [TestCase("-9", false)]
        [TestCase("-10", false)]
        [TestCase("-11", true)]
        [TestCase("1e", true)]
        public void SetTextValidates(string value, bool expected)
        {
            this.Sut.Text = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Sut));
        }

        [TestCase(8)]
        public void IncreaseCommand_CanExecuteChanged_OnIncrease(T value)
        {
            this.Sut.Value = value;
            var count = 0;
            this.Sut.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Assert.IsTrue(this.Sut.IncreaseCommand.CanExecute(null));
            this.Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual(0, count);
            Assert.IsTrue(this.Sut.IncreaseCommand.CanExecute(null));
            this.Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual(1, count);
            Assert.IsFalse(this.Sut.IncreaseCommand.CanExecute(null));
        }


        [TestCase(-9, 0)]
        [TestCase(9, 0)]
        [TestCase(11, 1)]
        public void IncreaseCommand_CanExecuteChanged_OnValueChanged(T newValue, int expected)
        {
            var count = 0;
            this.Sut.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            this.Sut.Value = newValue;
            Assert.AreEqual(expected, count);
        }

        [TestCase(-9, true)]
        [TestCase(-10, false)]
        [TestCase(-11, false)]
        public void DecreaseCommand_CanExecute_Value(T value, bool expected)
        {
            this.Sut.Value = value;
            Assert.AreEqual(expected, this.Sut.DecreaseCommand.CanExecute(null));
        }


        [TestCase(-8)]
        public void DecreaseCommand_CanExecute_OnDecrease(T value)
        {
            this.Sut.Value = value;
            var count = 0;
            this.Sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Assert.IsTrue(this.Sut.DecreaseCommand.CanExecute(null));
            this.Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(0, count);
            Assert.IsTrue(this.Sut.DecreaseCommand.CanExecute(null));
            this.Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(1, count);
            Assert.IsFalse(this.Sut.DecreaseCommand.CanExecute(null));
        }

        [TestCase(-11, 1)]
        [TestCase(-9, 0)]
        public void DecreaseCommand_CanExecuteChanged_OnValueChanged(T newValue, int expected)
        {
            var count = 0;
            this.Sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            this.Sut.Value = newValue;
            Assert.AreEqual(expected, count);
        }

        [TestCase(0, -1)]
        [TestCase(-9, -10)]
        [TestCase(-10, -10)]
        public void DecreaseCommand_Execute(T value, int expected)
        {
            this.Sut.Value = value;
            this.Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(expected, this.Sut.Value);
        }

        [TestCase(0, 1)]
        [TestCase(9, 10)]
        [TestCase(10, 10)]
        public void IncreaseCommand_Execute(T value, int expected)
        {
            this.Sut.Value = value;
            this.Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual(expected, this.Sut.Value);
        }


        [Test]
        public void SimpleSetValueFromText()
        {
            this.Sut.Text = "1";
            Assert.AreEqual(1, this.Sut.GetValue(NumericBox<T>.ValueProperty));
        }

        [TestCase(1)]
        public void TextUpdatesWhenValueChanges(T value)
        {
            this.Sut.SetValue(NumericBox<T>.ValueProperty, value);
            Assert.AreEqual("1", this.Sut.Text);
        }

        [Test]
        public void AppendDecimalDoesNotTruncateText()
        {
            this.Sut.Text = "1";
            Assert.AreEqual(1, this.Sut.GetValue(NumericBox<T>.ValueProperty));
            Assert.AreEqual("1", this.Sut.Text);
            var floats = new[] { typeof(double), typeof(float), typeof(decimal) };
            if (floats.Contains(typeof(T)))
            {
                this.Sut.Text = "1.";
                Assert.AreEqual(1, this.Sut.GetValue(NumericBox<T>.ValueProperty));
                Assert.AreEqual("1.", this.Sut.Text);

                this.Sut.Text = "1.0";
                Assert.AreEqual(1, this.Sut.GetValue(NumericBox<T>.ValueProperty));
                Assert.AreEqual("1.0", this.Sut.Text);
            }
        }

        [TestCase(1)]
        public void ErrorTextResetsValueFromSource(T expected)
        {
            var startValue = (T)this.Sut.GetValue(NumericBox<T>.ValueProperty);
            this.Sut.Text = "1";
            Assert.AreEqual(expected, this.Sut.GetValue(NumericBox<T>.ValueProperty));
            this.Sut.Text = "1e";
            var actual = (T)this.Sut.GetValue(NumericBox<T>.ValueProperty);
            var hasError = Validation.GetHasError(this.Box);
            Assert.AreEqual(startValue, actual);
            Assert.IsTrue(hasError);
        }
    }
}