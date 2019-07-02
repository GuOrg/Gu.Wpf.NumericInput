namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>A <see cref="IFormatAndCulture"/> for no parameters.</summary>
    public class NoParameterFormatAndCulture : FormatAndCulture<NoParameterFormatAndCulture>
    {
        private NoParameterFormatAndCulture(IFormatProvider formatProvider, string resourceKey)
            : base(formatProvider, resourceKey)
        {
        }

        /// <summary>Create a <see cref="NoParameterFormatAndCulture"/> for a resource in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/>.</summary>
        /// <param name="resourceKey">A key in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/>.</param>
        /// <returns>A <see cref="NoParameterFormatAndCulture"/> that can be used for formatting error messages.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="resourceKey"/> is not found in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/>.</exception>
        public static NoParameterFormatAndCulture CreateDefault(string resourceKey)
        {
            if (typeof(Properties.Resources).GetProperty(resourceKey, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) == null)
            {
                throw new ArgumentOutOfRangeException($"No resource found for key: {resourceKey}");
            }

            var formatAndCulture = new NoParameterFormatAndCulture(CultureInfo.InvariantCulture, resourceKey);
            formatAndCulture.Cache[CultureInfo.InvariantCulture] = formatAndCulture;
            return formatAndCulture;
        }

        /// <inheritdoc/>
        public override NoParameterFormatAndCulture GetOrCreate(IFormatProvider formatProvider)
        {
            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
            return this.Cache.GetOrAdd(culture, c => new NoParameterFormatAndCulture(formatProvider, this.ResourceKey));
        }

        /// <inheritdoc/>
        protected override void AssertFormat(IFormatProvider culture, string format)
        {
            if (!FormatString.IsValidFormat(format, out int itemCount, out bool? _))
            {
                throw new InvalidOperationException($"The format: '{format}' for culture: '{culture}' is not valid.");
            }

            if (itemCount != 0)
            {
                throw new InvalidOperationException($"The format: '{format}' for culture: '{culture}' is not for no arguments as expected.");
            }
        }
    }
}