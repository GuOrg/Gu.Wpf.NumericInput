using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gu.Wpf.NumericInput.Tests
{
    using System.Globalization;
    using NUnit.Framework;

    public abstract class FloatBaseTests<T> :NumericBoxTests<T> 
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        [Test]
        public void AppendDecimalDoesNotTruncateText()
        {
            Sut.Text = "1";
            Assert.AreEqual(1, Sut.GetValue(NumericBox<T>.ValueProperty));
            Assert.AreEqual("1", Sut.Text);
            Sut.Text = "1.";
            Assert.AreEqual(1, Sut.GetValue(NumericBox<T>.ValueProperty));
            Assert.AreEqual("1.", Sut.Text);

            Sut.Text = "1.0";
            Assert.AreEqual(1, Sut.GetValue(NumericBox<T>.ValueProperty));
            Assert.AreEqual("1.0", Sut.Text);
        }

        [TestCase("sv-SE","1,13",1.23)]
        public void Culture(string culture, string text, T expected )
        {
            Sut.Culture = new CultureInfo(culture);
            Sut.Text = text;
            Assert.AreEqual(expected, Sut.Value);
        }

        [TestCase(2, "1.234", "1.23", 1.234f), Explicit("Dunno how to test this, needs multibinding & converter")]
        public void ValueNotAffectedByDecimals(int decimals, string text, string expectedText, T expected)
        {
            Sut.Text = text;
            Sut.Decimals = decimals;
            Assert.AreEqual(expectedText, Sut.Text);
            Assert.AreEqual(expected, Sut.Value);
        }


    }
}
