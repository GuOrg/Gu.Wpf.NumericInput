namespace Gu.Wpf.NumericInput.Demo
{
    using System.Runtime.CompilerServices;

    public static class AutomationIds
    {
        public static readonly string MainWindow = Create();
        public static readonly string DebugTab = Create();
        public static readonly string DoubleBoxGroupBox = Create();
        public static readonly string InputBox = Create();
        public static readonly string ValueBlock = Create();
        public static readonly string TextSourceBlock = Create();
        public static readonly string StatusBlock = Create();
        public static readonly string VmValueBox = Create();
        public static readonly string IsReadonlyBox = Create();
        public static readonly string HasErrorsBox = Create();
        public static readonly string CanBeNullBox = Create();
        public static readonly string CultureBox = Create();
        public static readonly string DigitsBox = Create();
        public static readonly string StringFormatBox = Create();
        public static readonly string MaxBox = Create();
        public static readonly string MinBox = Create();
        public static readonly string AllowSpinnersBox = Create();
        public static readonly string IncrementBox = Create();
        public static readonly string RegexPatternBox = Create();
        public static readonly string FocusTab = Create();

        public static readonly string TextBoxes = Create();
        public static readonly string DoubleBoxes = Create();
        public static readonly string TextBox1 = Create();
        public static readonly string TextBox2 = Create();
        public static readonly string TextBox3 = Create();
        public static readonly string DoubleBox1 = Create();
        public static readonly string DoubleBox2 = Create();
        public static readonly string Spinners1 = Create();
        public static readonly string Spinners2 = Create();

        public static readonly string AllowLeadingWhiteBox = Create();
        public static readonly string AllowTrailingWhiteBox = Create();
        public static readonly string AllowLeadingSignBox = Create();
        public static readonly string AllowDecimalPointBox = Create();
        public static readonly string AllowThousandsBox = Create();
        public static readonly string AllowExponentBox = Create();


        public static readonly string SelectAllOnFocusBox = Create();
        public static readonly string SelectAllOnClickBox = Create();
        public static readonly string SelectAllOnDoubleClickBox = Create();
        public static readonly string MoveFocusOnEnterBox = Create();
        

        private static string Create([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
