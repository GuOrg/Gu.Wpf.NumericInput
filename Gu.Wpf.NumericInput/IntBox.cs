﻿namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="int"/>
    /// </summary>
    [ToolboxItem(true)]
    public class IntBox : NumericBox<int>
    {
        static IntBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntBox), new FrameworkPropertyMetadata(typeof(IntBox)));
            NumberStylesProperty.OverrideMetadata(
                typeof(IntBox),
                new PropertyMetadata(NumberStyles.Integer));
            IncrementProperty.OverrideMetadataWithDefaultValue<int>(typeof(IntBox), 1);
        }

        public IntBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool TryParse(string text, out int result)
        {
            return int.TryParse(text, this.NumberStyles, this.Culture, out result);
        }
    }
}