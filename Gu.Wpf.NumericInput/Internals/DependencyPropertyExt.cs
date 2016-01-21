namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    internal static class DependencyPropertyExt
    {
        internal static void OverrideMetadataWithDefaultValue<T>(
            this DependencyProperty property,
            Type forType,
            T defaultValue)
        {
            OverrideMetadataWithDefaultValue(property, property.OwnerType, forType, defaultValue);
        }

        internal static void OverrideMetadataWithDefaultValue<T>(
            this DependencyProperty property,
            Type ownerType,
            Type forType,
            T defaultValue)
        {
            var metadata = property.GetMetadata(ownerType);
            if (metadata.GetType() == typeof(PropertyMetadata))
            {
                property.OverrideMetadata(
                    forType,
                    new PropertyMetadata(
                        defaultValue,
                        metadata.PropertyChangedCallback,
                        metadata.CoerceValueCallback));
                return;
            }

            if (metadata.GetType() == typeof(FrameworkPropertyMetadata))
            {
                var fpm = (FrameworkPropertyMetadata)metadata;
                var flags = GetFlags(fpm);
                property.OverrideMetadata(
                    forType,
                    new FrameworkPropertyMetadata(
                        defaultValue,
                        flags,
                        metadata.PropertyChangedCallback,
                        metadata.CoerceValueCallback));
                return;
            }

            var message = $"Can only be called for properties with metadata of type {typeof(PropertyMetadata)}";
            throw new NotSupportedException(message);
        }

        internal static void OverrideMetadataWithCoerceMethod(this DependencyProperty property, Type forType, CoerceValueCallback coerceValueCallback)
        {
            var metadata = property.GetMetadata(property.OwnerType);
            if (metadata.GetType() == typeof(PropertyMetadata))
            {
                property.OverrideMetadata(
                    forType,
                    new PropertyMetadata(
                        metadata.DefaultValue,
                        metadata.PropertyChangedCallback,
                        coerceValueCallback));
                return;
            }

            if (metadata.GetType() == typeof(FrameworkPropertyMetadata))
            {
                var fpm = (FrameworkPropertyMetadata)metadata;
                var flags = GetFlags(fpm);
                property.OverrideMetadata(
                    forType,
                    new FrameworkPropertyMetadata(
                        metadata.DefaultValue,
                        flags,
                        metadata.PropertyChangedCallback,
                        coerceValueCallback));
                return;
            }

            var message = $"Can only be called for properties with metadata of type {typeof(PropertyMetadata)}";
            throw new NotSupportedException(message);
        }


        internal static void OverrideMetadataWithOptions(this DependencyProperty property, Type forType, FrameworkPropertyMetadataOptions options)
        {
            var metadata = property.GetMetadata(property.OwnerType);
            if (metadata.GetType() == typeof(PropertyMetadata))
            {
                property.OverrideMetadata(
                    forType,
                    new FrameworkPropertyMetadata(
                        metadata.DefaultValue,
                        options,
                        metadata.PropertyChangedCallback,
                        metadata.CoerceValueCallback,
                        false,
                        UpdateSourceTrigger.PropertyChanged));
                return;
            }

            if (metadata.GetType() == typeof(FrameworkPropertyMetadata))
            {
                var fpm = (FrameworkPropertyMetadata)metadata;
                property.OverrideMetadata(
                    forType,
                    new FrameworkPropertyMetadata(
                        metadata.DefaultValue,
                        options,
                        metadata.PropertyChangedCallback,
                        metadata.CoerceValueCallback,
                        fpm.IsAnimationProhibited,
                        fpm.DefaultUpdateSourceTrigger));
                return;
            }

            var message = $"Can only be called for properties with metadata of type {typeof(PropertyMetadata)}";
            throw new NotSupportedException(message);
        }

        private static FrameworkPropertyMetadataOptions GetFlags(FrameworkPropertyMetadata fpm)
        {
            FrameworkPropertyMetadataOptions flags = FrameworkPropertyMetadataOptions.None;
            if (fpm.AffectsMeasure)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsMeasure;
            }

            if (fpm.AffectsArrange)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsArrange;
            }

            if (fpm.AffectsParentMeasure)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsParentMeasure;
            }

            if (fpm.AffectsParentArrange)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsParentArrange;
            }

            if (fpm.AffectsRender)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsRender;
            }

            if (fpm.Inherits)
            {
                flags |= FrameworkPropertyMetadataOptions.Inherits;
            }

            if (fpm.OverridesInheritanceBehavior)
            {
                flags |= FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior;
            }

            if (fpm.IsNotDataBindable)
            {
                flags |= FrameworkPropertyMetadataOptions.NotDataBindable;
            }

            if (fpm.BindsTwoWayByDefault)
            {
                flags |= FrameworkPropertyMetadataOptions.BindsTwoWayByDefault;
            }

            if (fpm.Journal)
            {
                flags |= FrameworkPropertyMetadataOptions.Journal;
            }

            if (fpm.SubPropertiesDoNotAffectRender)
            {
                flags |= FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender;
            }

            return flags;
        }
    }
}
