namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class FormattedTextAdorner : Adorner
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(FormattedTextAdorner),
            new PropertyMetadata(default(string)));

        private readonly TextBlock textBlock;

        public FormattedTextAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.textBlock = new TextBlock
            {
                Name = BaseBox.FormattedName,
                VerticalAlignment = VerticalAlignment.Center
            };
            this.textBlock.Bind(TextBlock.TextProperty).OneWayTo(this, TextProperty);
            this.AddVisualChild(this.textBlock);
        }

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        protected override int VisualChildrenCount => this.textBlock != null ? 1 : 0;

        protected override Visual GetVisualChild(int index)
        {
            if (this.textBlock == null || index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.textBlock;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this.textBlock.Measure(constraint);
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size size)
        {
            var finalSize = base.ArrangeOverride(size);
            this.textBlock.Arrange(new Rect(new Point(0, 0), finalSize));
            return finalSize;
        }
    }
}
