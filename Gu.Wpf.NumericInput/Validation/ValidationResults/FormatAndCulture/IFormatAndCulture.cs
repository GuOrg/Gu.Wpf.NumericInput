namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;

    public interface IFormatAndCulture
    {
        /// <summary>Gets the name of the resource. I.e. Properties.Resources.ResourceManager.GetString(<see cref="IFormatAndCulture.ResourceKey"/>, <see cref="CultureInfo"/>).</summary>
        string ResourceKey { get; }

        /// <summary>Gets the culture for which the <see cref="IFormatAndCulture.Format"/> is for. If no localization is found <see cref="CultureInfo.InvariantCulture"/> will be used.</summary>
        IFormatProvider FormatProvider { get; }

#pragma warning disable CA1721 // Property names should not match get methods
        /// <summary>Gets the localized format string.</summary>
        string Format { get; }
#pragma warning restore CA1721 // Property names should not match get methods

        /// <summary>
        /// Gets the format in <paramref name="culture"/> if it exists.
        /// Returns format for <see cref="CultureInfo.InvariantCulture"/> if not exists.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <returns>A format string for a validation error message.</returns>
        string GetFormat(IFormatProvider culture);
    }
}
