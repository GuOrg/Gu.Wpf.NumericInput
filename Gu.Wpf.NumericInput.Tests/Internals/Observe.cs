namespace Gu.Wpf.NumericInput.Tests.Internals
{
    using System;
    using System.Windows;

    public static class Observe
    {
        public static IDisposable PropertyChanged(
            this DependencyObject source,
            DependencyProperty property,
            Action<DependencyPropertyChangedEventArgs> onChanged = null)
        {
            return new DependencyPropertyListener(source, property, onChanged);
        }
    }
}