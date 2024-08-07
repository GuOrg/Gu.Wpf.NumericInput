#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
namespace Gu.Wpf.NumericInput.Tests
{
    using System.Windows.Controls;

    using NUnit.Framework;

    [TestFixture]
    public abstract class BaseBoxTests
    {
        protected BaseBox? Box { get; set; }

        [TestCase("1", null!, false)]
        [TestCase("1", "", false)]
        [TestCase("1", "1", false)]
        [TestCase("1", "2", true)]
        public void PatternValidation(string text, string pattern, bool expected)
        {
            this.Box!.RegexPattern = pattern;
            this.Box.Text = text;
            Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected));
        }

        [TestCase("1", null!, false, "1", false)]
        [TestCase("1", null!, false, "2", true)]
        public void ValidatesOnPatternChanged(string text, string pattern1, bool expected1, string pattern2, bool expected2)
        {
            this.Box!.RegexPattern = pattern1;
            this.Box.Text = text;
            Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected1));

            this.Box.RegexPattern = pattern2;
            Assert.Multiple(() =>
            {
                Assert.That(Validation.GetHasError(this.Box), Is.EqualTo(expected2));

                Assert.That(this.Box.Status, Is.EqualTo(Status.Idle));
                Assert.That(this.Box.TextSource, Is.EqualTo(TextSource.UserInput));
            });
        }
    }
}
