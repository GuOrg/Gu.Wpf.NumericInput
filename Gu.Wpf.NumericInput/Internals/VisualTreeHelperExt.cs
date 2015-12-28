namespace Gu.Wpf.NumericInput
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    internal static class VisualTreeHelperExt
    {
        internal static IEnumerable<DependencyObject> Ancestors(this DependencyObject o)
        {
            var parent = VisualTreeHelper.GetParent(o);
            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }
    }
}