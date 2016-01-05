namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using NUnit.Framework;
    public abstract class NumericBoxTests<TBox, T>
        : BaseBoxTests
        where TBox : NumericBox<T>
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        public TBox Sut => (TBox)this.Box;

        protected abstract Func<TBox> Creator { get; }

        protected abstract T Max { get; }

        protected abstract T Min { get; }

        protected abstract T Increment { get; }

        internal DummyVm<T> Vm { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Box = this.Creator();
            this.Sut.IsReadOnly = false;
            this.Sut.MinValue = this.Min;
            this.Sut.MaxValue = this.Max;
            this.Sut.Increment = this.Increment;
            this.Vm = new DummyVm<T>();
            var binding = new Binding("Value")
            {
                Source = this.Vm,
                UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                Mode = BindingMode.TwoWay
            };
            var bindingExpression = BindingOperations.SetBinding(this.Sut, NumericBox<T>.ValueProperty, binding);
            this.Sut.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
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
            var box = this.Creator();
            Assert.AreEqual(1, box.Increment);

            var typeMin = (T)typeof(T).GetField("MinValue").GetValue(null);
            Assert.AreEqual(typeMin, box.MinLimit);
            Assert.IsNull(box.MinValue);

            var typeMax = (T)typeof(T).GetField("MaxValue").GetValue(null);
            Assert.AreEqual(typeMax, box.MaxLimit);
            Assert.IsNull(box.MaxValue);
        }

        [TestCase("9", true)]
        [TestCase("10", false)]
        [TestCase("11", false)]
        [TestCase("1e", false)]
        public void IncreaseCommand_CanExecute_Text(string text, bool expected)
        {
            this.Sut.Text = text;
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
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Sut));
            Assert.AreEqual(value.ToString(this.Sut.StringFormat, this.Sut.Culture), this.Sut.Text);
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Sut.TextSource);
        }

        [TestCase(9, false, 8, true)]
        [TestCase(10, false, 11, false)]
        [TestCase(11, true, 15, false)]
        public void SetMaxValidates(T value, bool expected, T newMax, bool expected2)
        {
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Sut));
            this.Sut.MaxValue = newMax;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Sut));
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Sut.TextSource);
        }

        [TestCase(-9, false, -8, true)]
        [TestCase(-10, false, -11, false)]
        [TestCase(-11, true, -15, false)]
        public void SetMinValidates(T value, bool expected, T newMax, bool expected2)
        {
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Sut));
            this.Sut.MinValue = newMax;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Sut));
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Sut.TextSource);
        }

        [TestCase("9", 9, false)]
        [TestCase("10", 10, false)]
        [TestCase("11", 0, true)]
        [TestCase("-9", -9, false)]
        [TestCase("-10", -10, false)]
        [TestCase("-11", 0, true)]
        [TestCase("1e", 0, true)]
        public void SetTextValidates(string text, T expectedValue, bool expected)
        {
            var changes = new List<DependencyPropertyChangedEventArgs>();
            using (this.Sut.PropertyChanged(NumericBox<T>.ValueProperty, x => changes.Add(x)))
            {
                this.Sut.Text = text;
                Assert.AreEqual(expected, Validation.GetHasError(this.Sut));
                if (expected)
                {
                    CollectionAssert.IsEmpty(changes);
                }
                else
                {
                    Assert.AreEqual(1, changes.Count);
                }
            }

            Assert.AreEqual(text, this.Sut.Text);
            Assert.AreEqual(this.Sut.Value, expectedValue);
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
        }

        [TestCase(1, "11", true, "1", false)]
        public void SetTextTwiceTest(T vmValue, string text1, bool expected1, string text2, bool expected2)
        {
            this.Vm.Value = vmValue;
            this.Sut.Text = text1;
            Assert.AreEqual(expected1, Validation.GetHasError(this.Sut));

            this.Sut.Text = text2;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Sut));
            //Assert.Fail("11 -> 1");
            Assert.AreEqual(Status.Idle, this.Sut.Status);
            Assert.AreEqual(TextSource.UserInput, this.Sut.TextSource);
        }

        [TestCase(8)]
        public void IncreaseCommand_CanExecuteChanged_OnIncrease(T value)
        {
            this.Sut.Value = value;
            var count = 0;
            this.Sut.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            ((ManualRelayCommand)this.Sut.IncreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);
            Assert.IsTrue(this.Sut.IncreaseCommand.CanExecute(null));

            this.Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual("9", this.Sut.Text);
            Assert.AreEqual(1, count);
            Assert.IsTrue(this.Sut.IncreaseCommand.CanExecute(null));

            this.Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual("10", this.Sut.Text);
            Assert.AreEqual(2, count);
            Assert.IsFalse(this.Sut.IncreaseCommand.CanExecute(null));
        }

        [TestCase(-9, 1)]
        [TestCase(9, 1)]
        [TestCase(11, 2)]
        [TestCase(-11, 1)]
        public void IncreaseCommand_CanExecuteChanged_OnValueChanged(T newValue, int expected)
        {
            var count = 0;
            this.Box.AllowSpinners = true;
            this.Sut.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            ((ManualRelayCommand)this.Sut.IncreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);

            this.Vm.Value = newValue;
            Assert.AreEqual(expected, count);
        }

        [TestCase("-9", true)]
        [TestCase("-10", false)]
        [TestCase("-11", false)]
        [TestCase("-1e", false)]
        public void DecreaseCommand_CanExecute_Text(string text, bool expected)
        {
            this.Sut.Text = text;
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
            Assert.AreEqual("-9", this.Sut.Text);
            Assert.AreEqual(1, count);
            Assert.IsTrue(this.Sut.DecreaseCommand.CanExecute(null));

            this.Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual("-10", this.Sut.Text);
            Assert.AreEqual(2, count);
            Assert.IsFalse(this.Sut.DecreaseCommand.CanExecute(null));
        }

        [TestCase("-11", 2)]
        [TestCase("-9", 1)]
        public void DecreaseCommand_CanExecuteChanged_OnTextChanged(string text, int expected)
        {
            var count = 0;
            this.Box.AllowSpinners = true;
            this.Sut.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            ((ManualRelayCommand)this.Sut.DecreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);
            this.Sut.Text = text;
            Assert.AreEqual(expected, count);
        }

        [TestCase("100", "99", 0)]
        [TestCase("0", "-1", -1)]
        [TestCase("-9", "-10", -10)]
        [TestCase("-10", "-10", -10)]
        public void DecreaseCommand_Execute(string text, string expectedText, T expected)
        {
            this.Sut.Text = text;
            this.Sut.DecreaseCommand.Execute(null);
            Assert.AreEqual(expectedText, this.Sut.Text);
            Assert.AreEqual(expected, this.Sut.Value);
        }

        [TestCase("-100", "-99", 0)]
        [TestCase("0", "1", 1)]
        [TestCase("9", "10", 10)]
        [TestCase("10", "10", 10)]
        public void IncreaseCommand_Execute(string text, string expectedText, T expected)
        {
            this.Sut.Text = text;
            this.Sut.IncreaseCommand.Execute(null);
            Assert.AreEqual(expectedText, this.Sut.Text);
            Assert.AreEqual(expected, this.Sut.Value);
        }

        [Test, Explicit("Can't test this yet")]
        public void IncreaseCommand_Execute_Undo()
        {
            Assert.IsTrue(this.Sut.IsUndoEnabled);
            Assert.AreEqual(100, this.Sut.UndoLimit);
            this.Sut.Text = "0";
            this.Sut.IncreaseCommand.Execute(null);
            Assert.IsTrue(this.Sut.CanUndo);
            this.Sut.Undo();
            Assert.AreEqual("0", this.Sut.Text);
        }

        [Test, Explicit("Can't test this yet")]
        public void DecreaseCommand_Execute_Undo()
        {
            // Not sure this can be tested
            var window = this.Sut.ShowInWindow();
            this.Sut.IsUndoEnabled = true;
            this.Sut.Text = "0";
            this.Sut.DecreaseCommand.Execute(null);
            Assert.IsTrue(this.Sut.CanUndo);
            this.Sut.Undo();
            Assert.AreEqual("0", this.Sut.Text);
            window.Close();
        }

        [Test]
        public void ValueUpdatesWhenTextIsSet()
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
        public void ValidationErrorResetsValue()
        {
            this.Sut.Text = "1";
            Assert.AreEqual(false, Validation.GetHasError(this.Box));
            Assert.AreEqual(1, this.Sut.Value);
            Assert.AreEqual(0, this.Vm.Value);

            this.Sut.Text = "1e";
            Assert.AreEqual(true, Validation.GetHasError(this.Box));
            Assert.AreEqual("1e", this.Sut.Text);
            Assert.AreEqual(this.Vm.Value, this.Sut.Value);
        }
    }
}