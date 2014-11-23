namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using NUnit.Framework;
    public abstract class NumericBoxTests<TBox,T>
        : BaseBoxTests
        where TBox : NumericBox<T>
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        protected abstract Func<TBox> Creator { get; }
        protected abstract T Max { get; }
        protected abstract T Min { get; }
        protected abstract T Increment { get; }

        private DummyVm<T> _vm;
        private BindingExpressionBase _bindingExpression;

        public TBox Sut { get { return (TBox)Box; } }

        [SetUp]
        public void SetUp()
        {
            Box = Creator();
            Sut.IsReadOnly = false;
            Sut.MinValue = Min;
            Sut.MaxValue = Max;
            Sut.Increment = Increment;
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
            var box = Creator();
            Assert.AreEqual(1, box.Increment);

            var typeMin = (T)typeof(T).GetField("MinValue").GetValue(null);
            Assert.AreEqual(typeMin, box.MinValue);

            var typeMax = (T)typeof(T).GetField("MaxValue").GetValue(null);
            Assert.AreEqual(typeMax, box.MaxValue);
        }

        [TestCase("9", true)]
        [TestCase("10", false)]
        [TestCase("11", false)]
        [TestCase("1e", false)]
        public void IncreaseCommand_CanExecute_Text(string text, bool expected)
        {
            Sut.Text = text;
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
            _vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(Sut));
            Assert.AreEqual(value.ToString(Sut.StringFormat, Sut.Culture), Sut.Text);
        }

        [TestCase(9, false, 8, true)]
        [TestCase(10, false, 11, false)]
        [TestCase(11, true, 15, false)]
        public void SetMaxValidates(T value, bool expected, T newMax, bool expected2)
        {
            _vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(Sut));
            Sut.MaxValue = newMax;
            Assert.AreEqual(expected2, Validation.GetHasError(Sut));
        }

        [TestCase(-9, false, -8, true)]
        [TestCase(-10, false, -11, false)]
        [TestCase(-11, true, -15, false)]
        public void SetMinValidates(T value, bool expected, T newMax, bool expected2)
        {
            _vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(Sut));
            Sut.MinValue = newMax;
            Assert.AreEqual(expected2, Validation.GetHasError(Sut));
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
            ((ManualRelayCommand)Sut.IncreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);
            Assert.IsTrue(Sut.IncreaseCommand.CanExecute(null));

            Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual("9", Sut.Text);
            Assert.AreEqual(1, count);
            Assert.IsTrue(Sut.IncreaseCommand.CanExecute(null));

            Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual("10", Sut.Text);
            Assert.AreEqual(2, count);
            Assert.IsFalse(Sut.IncreaseCommand.CanExecute(null));
        }

        [TestCase(-9, 1)]
        [TestCase(9, 1)]
        [TestCase(11, 2)]
        [TestCase(-11, 1)]
        public void IncreaseCommand_CanExecuteChanged_OnValueChanged(T newValue, int expected)
        {
            var count = 0;
            Sut.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            ((ManualRelayCommand)Sut.IncreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);

            _vm.Value = newValue;
            Assert.AreEqual(expected, count);
        }

        [TestCase("-9", true)]
        [TestCase("-10", false)]
        [TestCase("-11", false)]
        [TestCase("-1e", false)]
        public void DecreaseCommand_CanExecute_Text(string text, bool expected)
        {
            Sut.Text = text;
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
            Assert.AreEqual("-9", Sut.Text);
            Assert.AreEqual(1, count);
            Assert.IsTrue(Sut.DecreaseCommand.CanExecute(null));

            Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual("-10", Sut.Text);
            Assert.AreEqual(2, count);
            Assert.IsFalse(Sut.DecreaseCommand.CanExecute(null));
        }

        [TestCase("-11", 2)]
        [TestCase("-9", 1)]
        public void DecreaseCommand_CanExecuteChanged_OnTextChanged(string text, int expected)
        {
            var count = 0;
            Sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            ((ManualRelayCommand)Sut.DecreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);
            Sut.Text = text;
            Assert.AreEqual(expected, count);
        }

        [TestCase("100", "99", 0)]
        [TestCase("0", "-1", -1)]
        [TestCase("-9", "-10", -10)]
        [TestCase("-10", "-10", -10)]
        public void DecreaseCommand_Execute(string text, string expectedText, T expected)
        {
            Sut.Text = text;
            Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(expectedText, Sut.Text);
            Assert.AreEqual(expected, Sut.Value);
        }

        [TestCase("-100", "-99", 0)]
        [TestCase("0", "1", 1)]
        [TestCase("9", "10", 10)]
        [TestCase("10", "10", 10)]
        public void IncreaseCommand_Execute(string text, string expectedText, T expected)
        {
            Sut.Text = text;
            Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual(expectedText, Sut.Text);
            Assert.AreEqual(expected, Sut.Value);
        }

        [Test]
        public void IncreaseCommand_Execute_Undo()
        {
            // Not sure this can be tested
            Sut.IsUndoEnabled = true;
            Sut.Text = "0";
            Sut.IncreaseCommand.Execute(null);
            Assert.IsTrue(Sut.CanUndo);
            Sut.Undo();
            Assert.AreEqual("0", Sut.Text);
        }

        [Test]
        public void DecreaseCommand_Execute_Undo()
        {
            // Not sure this can be tested
            var window = Sut.ShowInWindow();
            Sut.IsUndoEnabled = true;
            Sut.Text = "0";
            Sut.DecreaseCommand.Execute(null);
            Assert.IsTrue(Sut.CanUndo);
            Sut.Undo();
            Assert.AreEqual("0", Sut.Text);
            window.Close();
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
    }
}