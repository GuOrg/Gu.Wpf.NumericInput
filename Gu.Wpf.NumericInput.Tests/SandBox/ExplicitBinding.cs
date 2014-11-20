namespace Gu.Wpf.NumericInput.Tests.SandBox
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Validation;

    using NUnit.Framework;

    public class ExplicitBindingSandbox
    {
        private Dummy _dummy;
        private BindingExpressionBase _bindingExpression;

        [SetUp]
        public void SetUp()
        {
            _dummy = new Dummy();
            var binding = new Binding("Value")
            {
                Source = _dummy,
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit
            };
            binding.ValidationRules.Add(new CanParse<double>(_dummy.CanParse));
            _bindingExpression = BindingOperations.SetBinding(_dummy, Dummy.TextProxyProperty, binding);
        }

        [Test]
        public void UpdateSource()
        {
            _dummy.TextProxy = "1";
            Assert.AreEqual(0, _dummy.Value);
            _bindingExpression.UpdateSource();
            Assert.AreEqual(1, _dummy.Value);
        }

        [Test]
        public void UpdateSourceValidates()
        {
            _dummy.TextProxy = "1e";
            Assert.AreEqual(0, _dummy.Value);
            _bindingExpression.UpdateSource();
            Assert.AreEqual(0, _dummy.Value);
            Assert.IsTrue(Validation.GetHasError(_dummy));

            _dummy.TextProxy = "1";
            _bindingExpression.UpdateSource();
            Assert.AreEqual(1, _dummy.Value);
            Assert.IsFalse(Validation.GetHasError(_dummy));
        }

        [Test]
        public void ExplicitValidate()
        {
            _dummy.TextProxy = "1e";
            _bindingExpression.ValidateWithoutUpdate();
            Assert.IsTrue(Validation.GetHasError(_dummy));
        }

        [Test]
        public void UpdateTarget()
        {
            _dummy.Value = 1.0;
            Assert.AreEqual(null, _dummy.TextProxy);
            _bindingExpression.UpdateTarget();
            Assert.AreEqual("1", _dummy.TextProxy);
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
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
        public string TextProxy
        {
            get
            {
                return (string)GetValue(TextProxyProperty);
            }
            set
            {
                SetValue(TextProxyProperty, value);
            }
        }

        public bool CanParse(string s)
        {
            double d;
            return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out d);
        }
    }
}
