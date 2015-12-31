namespace Gu.Wpf.NumericInput.Validation
{
    using System.Windows;

    /// <summary>
    /// Since we don't want text to be bindable we are binding to this attached property.
    /// This will enable wpf validation using a binding to the TextProxy attached property.
    /// </summary>
    internal static class ExplicitBinding
    {
        /// <summary>
        /// A proxy since Text is not bindable (we want it like this)
        /// </summary>
        internal static readonly DependencyProperty TextProxyProperty = DependencyProperty.RegisterAttached(
            "TextProxy",
            typeof(string),
            typeof(ExplicitBinding),
            new PropertyMetadata(default(string)));

        internal static void SetTextProxy(this BaseBox element, string value)
        {
            element.SetValue(TextProxyProperty, value);
        }

        internal static string GetTextProxy(this BaseBox element)
        {
            return (string)element.GetValue(TextProxyProperty);
        }
    }
}
