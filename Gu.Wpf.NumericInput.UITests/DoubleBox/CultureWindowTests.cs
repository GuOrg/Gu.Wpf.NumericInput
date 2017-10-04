namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class CultureWindowTests
    {
        private const string WindowName = "CultureWindow";

        [SetUp]
        public void SetUp()
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                window.FindButton("Reset").Invoke();
                window.WaitUntilResponsive();
            }
        }

        [OneTimeTearDown]
        public void OneTimeSetUp()
        {
            Application.KillLaunched(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"));
        }

        [TestCase("SpinnerDoubleBox", "1,234", "1,234")]
        [TestCase("InheritingCultureDoubleBox", "1,234", "1,234")]
        [TestCase("SvSeDoubleBox", "1,234", "1,234")]
        [TestCase("EnUsDoubleBox", "1.234", "1.234")]
        [TestCase("BoundCultureDoubleBox", "1,234", "1.234")]
        public void Formats(string name, string expectedSv, string expectedEn)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                var textBox = window.FindTextBox(name);
                Assert.AreEqual(expectedSv, textBox.Text);

                window.FindTextBox("CultureTextBox").Text = "en-us";
                Keyboard.Type(Key.TAB);
                Assert.AreEqual(expectedEn, textBox.Text);
            }
        }
    }
}