namespace Gu.Wpf.NumericInput.Tests.SandBox
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using NUnit.Framework;

    [Explicit]
    public class ExplicitBindingSandbox
    {
        private Dummy dummy;
        private BindingExpressionBase bindingExpression;

        [SetUp]
        public void SetUp()
        {
            this.dummy = new Dummy();
            var binding = new Binding("Value")
            {
                Source = this.dummy,
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit
            };
            binding.ValidationRules.Add(CanParse<double>.Default);
            this.bindingExpression = BindingOperations.SetBinding(this.dummy, Dummy.TextProxyProperty, binding);
        }

        [Test]
        public void UpdateSource()
        {
            this.dummy.TextProxy = "1";
            Assert.AreEqual(0, this.dummy.Value);
            this.bindingExpression.UpdateSource();
            Assert.AreEqual(1, this.dummy.Value);
        }

        [Test]
        public void UpdateSourceValidates()
        {
            this.dummy.TextProxy = "1e";
            Assert.AreEqual(0, this.dummy.Value);
            this.bindingExpression.UpdateSource();
            Assert.AreEqual(0, this.dummy.Value);
            Assert.IsTrue(Validation.GetHasError(this.dummy));

            this.dummy.TextProxy = "1";
            this.bindingExpression.UpdateSource();
            Assert.AreEqual(1, this.dummy.Value);
            Assert.IsFalse(Validation.GetHasError(this.dummy));
        }

        [Test]
        public void ExplicitValidate()
        {
            this.dummy.TextProxy = "1e";
            this.bindingExpression.ValidateWithoutUpdate();
            Assert.IsTrue(Validation.GetHasError(this.dummy));
        }

        [Test]
        public void UpdateTarget()
        {
            this.dummy.Value = 1.0;
            Assert.AreEqual(null, this.dummy.TextProxy);
            this.bindingExpression.UpdateTarget();
            Assert.AreEqual("1", this.dummy.TextProxy);
        }
    }

    public class Dummy : DependencyObject
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(Dummy),
            new PropertyMetadata(
                default(double)));

        public static readonly DependencyProperty TextProxyProperty = DependencyProperty.Register(
            "TextProxy",
            typeof(string),
            typeof(Dummy),
            new PropertyMetadata(
                default(string)));

        public double Value
        {
            get
            {
                return (double) this.GetValue(ValueProperty);
            }
            set
            {
                this.SetValue(ValueProperty, value);
            }
        }
        public string TextProxy
        {
            get
            {
                return (string) this.GetValue(TextProxyProperty);
            }
            set
            {
                this.SetValue(TextProxyProperty, value);
            }
        }

        public bool CanParse(string s)
        {
            double d;
            return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out d);
        }
    }
}
