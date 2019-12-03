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

        internal static IEnumerable<DependencyObject> NestedChildren(this DependencyObject parent)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                yield return child;
                if (VisualTreeHelper.GetChildrenCount(child) != 0)
                {
                    foreach (var nestedChild in NestedChildren(child))
                    {
                        yield return nestedChild;
                    }
                }
            }
        }

        internal static T? SingleOrNull<T>(this IEnumerable<object> items)
            where T : class
        {
            T? match = null;
            foreach (var item in items)
            {
                if (item is T temp)
                {
                    if (match != null)
                    {
                        return null;
                    }

                    match = temp;
                }
            }

            return match;
        }

        internal static IEnumerable<Visual> VisualChildren(this Visual parent)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                yield return (Visual)VisualTreeHelper.GetChild(parent, i);
            }
        }
    }
}
