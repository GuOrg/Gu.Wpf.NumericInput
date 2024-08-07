#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
namespace Gu.Wpf.NumericInput.Tests
{
    using System;
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
        protected new TBox Box => (TBox)base.Box!;

        protected abstract Func<TBox> Creator { get; }

        protected abstract T Max { get; }

        protected abstract T Min { get; }

        protected abstract T Increment { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected DummyVm<T> Vm { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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
                Mode = BindingMode.TwoWay,
            };
            _ = BindingOperations.SetBinding(this.Box, NumericBox<T>.ValueProperty, binding);
            this.Box.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        [Test]
        public void Defaults()
        {
            var box = this.Creator();
            Assert.That(box.Increment, Is.EqualTo(1));
            var typeMin = (T)typeof(T).GetField("MinValue")!.GetValue(null)!;
            Assert.That(box.MinLimit, Is.EqualTo(typeMin));
            Assert.That(box.MinValue, Is.Null);

            var typeMax = (T)typeof(T).GetField("MaxValue")!.GetValue(null)!;
            Assert.That(box.MaxLimit, Is.EqualTo(typeMax));
            Assert.That(box.MaxValue, Is.Null);
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
            Assert.Multiple(() =>
            {
                Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected));
                Assert.That(this.Box.Text, Is.EqualTo(value.ToString(this.Box.StringFormat, this.Box.Culture)));
                Assert.That(this.Box.Status, Is.EqualTo(Status.Idle));
                Assert.That(this.Box.TextSource, Is.EqualTo(TextSource.ValueBinding));
            });
        }

        [TestCase(9, false, 8, true)]
        [TestCase(10, false, 11, false)]
        [TestCase(11, true, 15, false)]
        public void SetMaxValidates(T value, bool expected, T newMax, bool expected2)
        {
            this.Vm.Value = value;
            Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected));
            this.Box.MaxValue = newMax;
            Assert.Multiple(() =>
            {
                Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected2));
                Assert.That(this.Box.Status, Is.EqualTo(Status.Idle));
                Assert.That(this.Box.TextSource, Is.EqualTo(TextSource.ValueBinding));
            });
        }

        [TestCase(-9, false, -8, true)]
        [TestCase(-10, false, -11, false)]
        [TestCase(-11, true, -15, false)]
        public void SetMinValidates(T value, bool expected, T newMax, bool expected2)
        {
            this.Vm.Value = value;
            Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected));
            this.Box.MinValue = newMax;
            Assert.Multiple(() =>
            {
                Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected2));
                Assert.That(this.Box.Status, Is.EqualTo(Status.Idle));
                Assert.That(this.Box.TextSource, Is.EqualTo(TextSource.ValueBinding));
            });
        }

        [TestCase(1, "11", true, "1", false)]
        public void SetTextTwiceTest(T vmValue, string text1, bool expected1, string text2, bool expected2)
        {
            this.Vm.Value = vmValue;
            this.Box.Text = text1;
            Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected1));

            this.Box.Text = text2;
            Assert.Multiple(() =>
            {
                Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected2));
                ////Assert.Fail("11 -> 1");
                Assert.That(this.Box.Status, Is.EqualTo(Status.Idle));
                Assert.That(this.Box.TextSource, Is.EqualTo(TextSource.UserInput));
            });
        }

        [Test]
        public void ValueUpdatesWhenTextIsSet()
        {
            this.Box.Text = "1";
            Assert.That(this.Box.GetValue(NumericBox<T>.ValueProperty), Is.EqualTo(1));
        }

        [TestCase(1)]
        public void TextUpdatesWhenValueChanges(T value)
        {
#pragma warning disable WPF0014 // SetValue must use registered type.
            this.Box.SetValue(NumericBox<T>.ValueProperty, value);
#pragma warning restore WPF0014 // SetValue must use registered type.
            Assert.That(this.Box.Text, Is.EqualTo("1"));
        }

        [Test]
        public void ValidationErrorResetsValue()
        {
            this.Box.Text = "1";
            Assert.Multiple(() =>
            {
                Assert.That(Validation.GetHasError(base.Box), Is.EqualTo(false));
                Assert.That(this.Box.Value, Is.EqualTo(1));
                Assert.That(this.Vm.Value, Is.EqualTo(null));
            });

            this.Box.Text = "1e";
            Assert.Multiple(() =>
            {
                Assert.That(Validation.GetHasError(base.Box), Is.EqualTo(true));
                Assert.That(this.Box.Text, Is.EqualTo("1e"));
                Assert.That(this.Box.Value, Is.EqualTo(this.Vm.Value));
            });
        }

        [TestCase("-100", "-99", 0)]
        [TestCase("0", "1", 1)]
        [TestCase("9", "10", 10)]
        [TestCase("10", "10", 10)]
        public void IncreaseCommandExecute(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.IncreaseCommand!.Execute(null);
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.Text, Is.EqualTo(expectedText));
                Assert.That(this.Box.Value, Is.EqualTo(expected));
                Assert.That(this.Vm.Value, Is.EqualTo(this.Box.Parse("0")));
            });
        }

        [TestCase("-100", "-99", 0)]
        [TestCase("-10", "-9", -9)]
        [TestCase("0", "1", 1)]
        [TestCase("9", "10", 10)]
        [TestCase("10", "10", 10)]
        public void IncreaseCommandExecuteSpinUpdateModePropertyChanged(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.SpinUpdateMode = SpinUpdateMode.PropertyChanged;
            this.Box.IncreaseCommand!.Execute(null);
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.Text, Is.EqualTo(expectedText));
                Assert.That(this.Box.Value, Is.EqualTo(expected));
                Assert.That(this.Vm.Value, Is.EqualTo(this.Box.Parse(expected.ToString(CultureInfo.InvariantCulture))));
            });
        }

        [TestCase("9", true)]
        [TestCase("10", false)]
        [TestCase("11", false)]
        [TestCase("1e", false)]
        public void IncreaseCommandCanExecuteOnUserInput(string text, bool expected)
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (_, __) => count++;
            this.Box.Text = text;
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.IncreaseCommand.CanExecute(null), Is.EqualTo(expected));
                Assert.That(count, Is.EqualTo(1));
            });

            this.Box.AllowSpinners = false;
            Assert.That(count, Is.EqualTo(2));

            this.Box.Text = string.Empty;
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.IncreaseCommand.CanExecute(null), Is.EqualTo(false));
                Assert.That(count, Is.EqualTo(2));
            });
        }

        [Test]
        public void IncreaseCommandCanExecuteRaiseExplicit()
        {
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (_, __) => count++;
            ((ManualRelayCommand)this.Box.IncreaseCommand).RaiseCanExecuteChanged();
            Assert.That(count, Is.EqualTo(1));
        }

        [TestCase(8)]
        public void IncreaseCommandCanExecuteChangedOnIncrease(T value)
        {
            this.Box.AllowSpinners = true;
            this.Box.Value = value;
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (_, __) => count++;
            Assert.That(this.Box.IncreaseCommand.CanExecute(null), Is.True);

            this.Box.IncreaseCommand.Execute(null);
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.Text, Is.EqualTo("9"));
                Assert.That(count, Is.EqualTo(1));
            });
            Assert.That(this.Box.IncreaseCommand.CanExecute(null), Is.True);

            this.Box.IncreaseCommand.Execute(null);
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.Text, Is.EqualTo("10"));
                Assert.That(count, Is.EqualTo(2));
            });
            Assert.That(this.Box.IncreaseCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void IncreaseCommandCanExecuteChangedOnValueChanged()
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (sender, args) => count++;
            this.Vm.Value = this.Box.Parse("1");
            Assert.That(count, Is.EqualTo(1));

            this.Box.AllowSpinners = false;
            Assert.That(count, Is.EqualTo(2));

            this.Vm.Value = this.Box.Parse("2");
            Assert.That(count, Is.EqualTo(2));
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IncreaseCommandCanExecuteIsReadonly(bool @readonly, bool expected)
        {
            this.Box.AllowSpinners = true;
            base.Box!.Text = "0";
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (_, __) => count++;
            base.Box.IsReadOnly = @readonly;
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.IncreaseCommand.CanExecute(null), Is.EqualTo(expected));
                Assert.That(count, Is.EqualTo(@readonly ? 1 : 0));
            });
        }

        [TestCase("100", "99", 0)]
        [TestCase("0", "-1", -1)]
        [TestCase("-9", "-10", -10)]
        [TestCase("-10", "-10", -10)]
        public void DecreaseCommandExecute(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.DecreaseCommand!.Execute(null);
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.Text, Is.EqualTo(expectedText));
                Assert.That(this.Box.Value, Is.EqualTo(expected));
            });
        }

        [TestCase("100", "99", 0)]
        [TestCase("10", "9", 9)]
        [TestCase("0", "-1", -1)]
        [TestCase("-9", "-10", -10)]
        [TestCase("-10", "-10", -10)]
        public void DecreaseCommandExecuteSpinUpdateModePropertyChanged(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.SpinUpdateMode = SpinUpdateMode.PropertyChanged;
            this.Box.DecreaseCommand!.Execute(null);
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.Text, Is.EqualTo(expectedText));
                Assert.That(this.Box.Value, Is.EqualTo(expected));
                Assert.That(this.Vm.Value, Is.EqualTo(this.Box.Parse(expected.ToString(CultureInfo.InvariantCulture))));
            });
        }

        [Test]
        public void DecreaseCommandCanExecuteRaiseExplicit()
        {
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (_, __) => count++;
            ((ManualRelayCommand)this.Box.DecreaseCommand).RaiseCanExecuteChanged();
            Assert.That(count, Is.EqualTo(1));
        }

        [TestCase("-9", true)]
        [TestCase("-10", false)]
        [TestCase("-11", false)]
        [TestCase("-1e", false)]
        public void DecreaseCommandCanExecuteOnUserInput(string text, bool expected)
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (_, __) => count++;
            this.Box.Text = text;
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.DecreaseCommand.CanExecute(null), Is.EqualTo(expected));
                Assert.That(count, Is.EqualTo(1));
            });

            this.Box.AllowSpinners = false;
            Assert.That(count, Is.EqualTo(2));

            this.Box.Text = string.Empty;
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.DecreaseCommand.CanExecute(null), Is.EqualTo(false));
                Assert.That(count, Is.EqualTo(2));
            });
        }

        [TestCase(-8)]
        public void DecreaseCommandCanExecuteOnDecrease(T value)
        {
            this.Box.AllowSpinners = true;
            this.Box.Value = value;
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (sender, args) => count++;
            Assert.That(this.Box.DecreaseCommand.CanExecute(null), Is.True);

            this.Box.DecreaseCommand.Execute(null);
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.Text, Is.EqualTo("-9"));
                Assert.That(count, Is.EqualTo(1));
            });
            Assert.That(this.Box.DecreaseCommand.CanExecute(null), Is.True);

            this.Box.DecreaseCommand.Execute(null);
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.Text, Is.EqualTo("-10"));
                Assert.That(count, Is.EqualTo(2));
            });
            Assert.That(this.Box.DecreaseCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void DecreaseCommandCanExecuteChangedOnValueChanged()
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (sender, args) => count++;
            this.Vm.Value = this.Box.Parse("1");
            Assert.That(count, Is.EqualTo(1));

            this.Box.AllowSpinners = false;
            Assert.That(count, Is.EqualTo(2));

            this.Vm.Value = this.Box.Parse("2");
            Assert.That(count, Is.EqualTo(2));
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void DecreaseCommandCanExecuteIsReadonly(bool @readonly, bool expected)
        {
            this.Box.AllowSpinners = true;
            this.Box.Text = "0";
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (_, __) => count++;
            this.Box.IsReadOnly = @readonly;
            Assert.Multiple(() =>
            {
                Assert.That(this.Box.DecreaseCommand.CanExecute(null), Is.EqualTo(expected));
                Assert.That(count, Is.EqualTo(@readonly ? 1 : 0));
            });
        }
    }
}
