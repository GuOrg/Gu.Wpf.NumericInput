namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;

    public interface IFormatAndCulture
    {
        /// <summary>The name of the resource. I.e. Properties.Resources.ResourceManager.GetString(<see cref="IFormatAndCulture.ResourceKey"/>, <see cref="CultureInfo"/>)</summary>
        string ResourceKey { get; }

        /// <summary>The culture for which the <see cref="IFormatAndCulture.Format"/> is for. If no localization is found <see cref="CultureInfo.InvariantCulture"/> will be used.</summary>
        IFormatProvider FormatProvider { get; }

        /// <summary>Gets the localized format string.</summary>
        string Format { get; }

        string GetFormat(IFormatProvider culture);
    }
}