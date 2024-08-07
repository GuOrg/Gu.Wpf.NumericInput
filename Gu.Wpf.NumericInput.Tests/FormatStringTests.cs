namespace Gu.Wpf.NumericInput.Tests
{
    using NUnit.Framework;

    public class FormatStringTests
    {
        [TestCase(null!, false)]
        [TestCase("", false)]
        [TestCase("First", false)]
        [TestCase("First: {0}", true)]
        [TestCase("First: {0:N}", true)]
        [TestCase("First: {0} ", true)]
        [TestCase("First: {0}, Second: {0:N}", true)]
        [TestCase("First: {0:F2 }, Second: {0:N}", true)]
        [TestCase("First: {{{0}}}", true)]
        [TestCase("First: {{{0:F3}}}", true)]
        [TestCase("First: {0", false)]
        [TestCase("First: 0}", false)]
        [TestCase("First: {{0}}", false)]
        [TestCase("First: {{0}", false)]
        [TestCase("First: {0}}", false)]
        [TestCase("First: {1}", false)]
        [TestCase("First: {0}, Second: {1}", true)]
        [TestCase("First: {1}, Second: {0}", true)]
        [TestCase("First: {0}, First: {0:N} Second: {1}", true)]
        [TestCase("First: {0}, Second: {1:N}", true)]
        [TestCase("First: {0}, Second: {2}", false)]
        [TestCase("First: {1}, Second: {2}", false)]
        [TestCase("First: {1}, Second: {1}", false)]
        [TestCase("First: {0N}", false)]
        public void IsFormatString(string text, bool expected)
        {
            Assert.That(FormatString.IsFormatString(text), Is.EqualTo(expected));
        }

        [TestCase(null!, 0, true)]
        [TestCase(null!, 1, false)]
        [TestCase("", 0, true)]
        [TestCase("", 1, false)]
        [TestCase("First", 0, true)]
        [TestCase("First", 1, false)]
        [TestCase("First: {0}", 1, true)]
        [TestCase("First: {0} ", 1, true)]
        [TestCase("First: {0}, Second: {0:N}", 1, true)]
        [TestCase("First: {0:F2 }, Second: {0:N}", 1, true)]
        [TestCase("First: {{{0}}}", 1, true)]
        [TestCase("First: {{{0:F3}}}", 1, true)]
        [TestCase("First: {0", 1, false)]
        [TestCase("First: 0}", 1, false)]
        [TestCase("First: {{0}}", 1, false)]
        [TestCase("First: {{0}", 1, false)]
        [TestCase("First: {0}}", 1, false)]
        [TestCase("First: {1}", 1, false)]
        [TestCase("First: {0}, Second: {1}", 2, true)]
        [TestCase("First: {1}, Second: {0}", 2, true)]
        [TestCase("First: {0}, First: {0:N} Second: {1}", 2, true)]
        [TestCase("First: {0}, Second: {1:N}", 2, true)]
        [TestCase("First: {0}, Second: {2}", 2, false)]
        [TestCase("First: {1}, Second: {2}", 2, false)]
        [TestCase("First: {1}, Second: {2}", 1, false)]
        [TestCase("First: {1}, Second: {2}", 3, false)]
        [TestCase("First: {0:N}", 1, true)]
        [TestCase("First: {0N}", 1, false)]
        public void IsValidFormatString(string text, int numberOfArguments, bool expected)
        {
            Assert.That(FormatString.IsValidFormatString(text, numberOfArguments), Is.EqualTo(expected));
        }

        [TestCase("", true, 0, false)]
        [TestCase("First", true, 0, false)]
        [TestCase("First: {{0}}", true, 0, false)]
        [TestCase("First: {{0}", false, -1, false)]
        [TestCase("First: {0}}", false, -1, false)]
        [TestCase("First: {0}", true, 1, false)]
        [TestCase("First: {{{0}}}", true, 1, false)]
        [TestCase("First: {{{0:F3}}}", true, 1, true)]
        [TestCase("First: {0:F2 }, Second: {0:N}", true, 1, true)]
        [TestCase("First: {0}, Second: {0:N}", true, 1, true)]
        [TestCase("First: {0}, Second: {1}", true, 2, false)]
        [TestCase("First: {1}, Second: {0}", true, 2, false)]
        [TestCase("First: {1}, Second: {2}", false, -1, false)]
        [TestCase("First: {0}, First: {0:N} Second: {1}", true, 2, true)]
        [TestCase("First: {0}, Second: {1:N}", true, 2, true)]
        [TestCase("First: {0:N}", true, 1, true)]
        [TestCase("First: {0} ", true, 1, false)]
        [TestCase("First: {1}", false, -1, false)]
        [TestCase("First: {0N}", false, -1, null)]
        public void IsValidFormatWithOutParams(string text, bool expected, int expectedIndex, bool? expectedFormat)
        {
            Assert.That(FormatString.IsValidFormat(text, out int count, out bool? anyItemHasFormat), Is.EqualTo(expected));
            Assert.That(count, Is.EqualTo(expectedIndex));
            Assert.That(anyItemHasFormat, Is.EqualTo(expectedFormat));
        }
    }
}
