namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;

    internal static class DependencyPropertyExt
    {
        internal static void OverrideMetadataWithDefaultValue<T>(this DependencyProperty property, Type forType, T defaultValue)
        {
            var metadata = property.GetMetadata(property.OwnerType);
            if (metadata.GetType() != typeof(PropertyMetadata))
            {
                var message = $"Can only be called for properties with metadata of type {typeof(PropertyMetadata)}";
                throw new NotSupportedException(message);
            }

            property.OverrideMetadata(
                forType,
                new PropertyMetadata(
                    defaultValue,
                    metadata.PropertyChangedCallback,
                    metadata.CoerceValueCallback));
        }
    }
}
