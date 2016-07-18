namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Gu.Wpf.NumericInput.Tests.Internals;
    using NUnit.Framework;

    public abstract class NumericBoxTests<TBox, T>
        : BaseBoxTests
        where TBox : NumericBox<T>
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        public new TBox Box => (TBox)base.Box;

        protected abstract Func<TBox> Creator { get; }

        protected abstract T Max { get; }

        protected abstract T Min { get; }

        protected abstract T Increment { get; }

        internal DummyVm<T> Vm { get; private set; }

        [SetUp]
        public void SetUp()
        {
            var enUs = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = enUs;
            Thread.CurrentThread.CurrentCulture = enUs;
            base.Box = this.Creator();
            this.Box.IsReadOnly = false;
            this.Box.MinValue = this.Min;
            this.Box.MaxValue = this.Max;
            this.Box.Increment = this.Increment;
            this.Box.Culture = Thread.CurrentThread.CurrentUICulture;
            this.Vm = new DummyVm<T>();
            var binding = new Binding("Value")
            {
                Source = this.Vm,
                UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(this.Box, NumericBox<T>.ValueProperty, binding);
            this.Box.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
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

        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, true)]
        [TestCase(-9, false)]
        [TestCase(-10, false)]
        [TestCase(-11, true)]
        public void SetValueValidates(T value, bool expected)
        {
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Box));
            Assert.AreEqual(value.ToString(this.Box.StringFormat, this.Box.Culture), this.Box.Text);
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Box.TextSource);
        }

        [TestCase(9, false, 8, true)]
        [TestCase(10, false, 11, false)]
        [TestCase(11, true, 15, false)]
        public void SetMaxValidates(T value, bool expected, T newMax, bool expected2)
        {
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Box));
            this.Box.MaxValue = newMax;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Box));
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Box.TextSource);
        }

        [TestCase(-9, false, -8, true)]
        [TestCase(-10, false, -11, false)]
        [TestCase(-11, true, -15, false)]
        public void SetMinValidates(T value, bool expected, T newMax, bool expected2)
        {
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Box));
            this.Box.MinValue = newMax;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Box));
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Box.TextSource);
        }

        [TestCase(1, "11", true, "1", false)]
        public void SetTextTwiceTest(T vmValue, string text1, bool expected1, string text2, bool expected2)
        {
            this.Vm.Value = vmValue;
            this.Box.Text = text1;
            Assert.AreEqual(expected1, Validation.GetHasError(this.Box));

            this.Box.Text = text2;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Box));
            //Assert.Fail("11 -> 1");
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
        }

        [Test]
        public void ValueUpdatesWhenTextIsSet()
        {
            this.Box.Text = "1";
            Assert.AreEqual(1, this.Box.GetValue(NumericBox<T>.ValueProperty));
        }

        [TestCase(1)]
        public void TextUpdatesWhenValueChanges(T value)
        {
            this.Box.SetValue(NumericBox<T>.ValueProperty, value);
            Assert.AreEqual("1", this.Box.Text);
        }

        [Test]
        public void ValidationErrorResetsValue()
        {
            this.Box.Text = "1";
            Assert.AreEqual(false, Validation.GetHasError(base.Box));
            Assert.AreEqual(1, this.Box.Value);
            Assert.AreEqual(null, this.Vm.Value);

            this.Box.Text = "1e";
            Assert.AreEqual(true, Validation.GetHasError(base.Box));
            Assert.AreEqual("1e", this.Box.Text);
            Assert.AreEqual(this.Vm.Value, this.Box.Value);
        }

        [TestCase("-100", "-99", 0)]
        [TestCase("0", "1", 1)]
        [TestCase("9", "10", 10)]
        [TestCase("10", "10", 10)]
        public void IncreaseCommand_Execute(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.IncreaseCommand.Execute(null);
            Assert.AreEqual(expectedText, this.Box.Text);
            Assert.AreEqual(expected, this.Box.Value);
        }

        [TestCase("9", true)]
        [TestCase("10", false)]
        [TestCase("11", false)]
        [TestCase("1e", false)]
        public void IncreaseCommand_CanExecute_OnUserInput(string text, bool expected)
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.IncreaseCommand.CanExecuteChanged += (_, __) => count++;
            this.Box.Text = text;
            Assert.AreEqual(expected, this.Box.IncreaseCommand.CanExecute(null));
            Assert.AreEqual(1, count);

            this.Box.AllowSpinners = false;
            Assert.AreEqual(2, count);

            this.Box.Text = string.Empty;
            Assert.AreEqual(false, this.Box.IncreaseCommand.CanExecute(null));
            Assert.AreEqual(2, count);
        }

        [Test]
        public void IncreaseCommand_CanExecute_RaiseExplicit()
        {
            var count = 0;
            this.Box.IncreaseCommand.CanExecuteChanged += (_, __) => count++;
            ((ManualRelayCommand)this.Box.IncreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);
        }

        [TestCase(8)]
        public void IncreaseCommand_CanExecuteChanged_OnIncrease(T value)
        {
            this.Box.AllowSpinners = true;
            this.Box.Value = value;
            var count = 0;
            this.Box.IncreaseCommand.CanExecuteChanged += (_, __) => count++;
            Assert.IsTrue(this.Box.IncreaseCommand.CanExecute(null));

            this.Box.IncreaseCommand.Execute(null);
            Assert.AreEqual("9", this.Box.Text);
            Assert.AreEqual(1, count);
            Assert.IsTrue(this.Box.IncreaseCommand.CanExecute(null));

            this.Box.IncreaseCommand.Execute(null);
            Assert.AreEqual("10", this.Box.Text);
            Assert.AreEqual(2, count);
            Assert.IsFalse(this.Box.IncreaseCommand.CanExecute(null));
        }

        [Test]
        public void IncreaseCommand_CanExecuteChanged_OnValueChanged()
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.IncreaseCommand.CanExecuteChanged += (sender, args) => count++;
            this.Vm.Value = this.Box.Parse("1");
            Assert.AreEqual(1, count);

            this.Box.AllowSpinners = false;
            Assert.AreEqual(2, count);

            this.Vm.Value = this.Box.Parse("2");
            Assert.AreEqual(2, count);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IncreaseCommand_CanExecute_IsReadonly(bool @readonly, bool expected)
        {
            this.Box.AllowSpinners = true;
            base.Box.Text = "0";
            var count = 0;
            this.Box.IncreaseCommand.CanExecuteChanged += (_, __) => count++;
            base.Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, this.Box.IncreaseCommand.CanExecute(null));
            Assert.AreEqual(@readonly ? 1 : 0, count);
        }

        [TestCase("100", "99", 0)]
        [TestCase("0", "-1", -1)]
        [TestCase("-9", "-10", -10)]
        [TestCase("-10", "-10", -10)]
        public void DecreaseCommand_Execute(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.DecreaseCommand.Execute(null);
            Assert.AreEqual(expectedText, this.Box.Text);
            Assert.AreEqual(expected, this.Box.Value);
        }

        [Test]
        public void DecreaseCommand_CanExecute_RaiseExplicit()
        {
            var count = 0;
            this.Box.DecreaseCommand.CanExecuteChanged += (_, __) => count++;
            ((ManualRelayCommand)this.Box.DecreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);
        }

        [TestCase("-9", true)]
        [TestCase("-10", false)]
        [TestCase("-11", false)]
        [TestCase("-1e", false)]
        public void DecreaseCommand_CanExecute_OnUserInput(string text, bool expected)
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.DecreaseCommand.CanExecuteChanged += (_, __) => count++;
            this.Box.Text = text;
            Assert.AreEqual(expected, this.Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(1, count);

            this.Box.AllowSpinners = false;
            Assert.AreEqual(2, count);

            this.Box.Text = string.Empty;
            Assert.AreEqual(false, this.Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(2, count);
        }

        [TestCase(-8)]
        public void DecreaseCommand_CanExecute_OnDecrease(T value)
        {
            this.Box.AllowSpinners = true;
            this.Box.Value = value;
            var count = 0;
            this.Box.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            Assert.IsTrue(this.Box.DecreaseCommand.CanExecute(null));

            this.Box.DecreaseCommand.Execute(null);
            Assert.AreEqual("-9", this.Box.Text);
            Assert.AreEqual(1, count);
            Assert.IsTrue(this.Box.DecreaseCommand.CanExecute(null));

            this.Box.DecreaseCommand.Execute(null);
            Assert.AreEqual("-10", this.Box.Text);
            Assert.AreEqual(2, count);
            Assert.IsFalse(this.Box.DecreaseCommand.CanExecute(null));
        }

        [Test]
        public void DecreaseCommand_CanExecuteChanged_OnValueChanged()
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.DecreaseCommand.CanExecuteChanged += (sender, args) => count++;
            this.Vm.Value = this.Box.Parse("1");
            Assert.AreEqual(1, count);

            this.Box.AllowSpinners = false;
            Assert.AreEqual(2, count);

            this.Vm.Value = this.Box.Parse("2");
            Assert.AreEqual(2, count);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void DecreaseCommand_CanExecute_IsReadonly(bool @readonly, bool expected)
        {
            this.Box.AllowSpinners = true;
            base.Box.Text = "0";
            var count = 0;
            this.Box.DecreaseCommand.CanExecuteChanged += (_, __) => count++;
            base.Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, this.Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(@readonly ? 1 : 0, count);
        }
    }
}