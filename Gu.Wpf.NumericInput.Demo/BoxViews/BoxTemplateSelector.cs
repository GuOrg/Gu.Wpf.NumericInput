namespace Gu.Wpf.NumericInput.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public class BoxTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Double { get; set; }

        public DataTemplate DoubleTweaked { get; set; }

        public DataTemplate Int { get; set; }

        public DataTemplate IntTweaked { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }

            var boxVm = ((IBoxVm)item);
            var type =boxVm.Type;
            if (type == typeof(IntBox))
            {
                if (boxVm.Configurable)
                {
                    return IntTweaked;
                }
                return Int;
            }

            if (type == typeof(DoubleBox))
            {
                if (boxVm.Configurable)
                {
                    return DoubleTweaked;
                }
                return this.Double;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
