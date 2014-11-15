namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    [ToolboxItem(false)]
    public abstract class CommonUpDown<T> : NumericUpDown<T>
        where T : struct, IComparable<T>
    {
        protected static void UpdateMetadata(Type type, T increment, T minValue, T maxValue, int decimals)
        {
            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));

            IncrementProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
            MaxValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(maxValue));
            MinValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(minValue));
            DecimalsProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(decimals));
        }

        protected override void Increase()
        {
            this.Validate();
            if (this.CanIncrease())
            {
                this.Value = this.IncrementValue(this.Value, this.Increment);
            }
        }

        protected override void Decrease()
        {
            this.Validate();
            if (this.CanDecrease())
            {
                this.Value = this.DecrementValue(this.Value, this.Increment);
            }
        }

        protected abstract T IncrementValue(T value, T increment);

        protected abstract T DecrementValue(T value, T increment);
    }
}