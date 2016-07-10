namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Reflection;

    public class OneParameterFormatAndCulture : FormatAndCulture<OneParameterFormatAndCulture>
    {
        private OneParameterFormatAndCulture(IFormatProvider formatProvider, string resourceKey)
            : base(formatProvider, resourceKey)
        {
        }

        /// <summary>Create a <see cref="OneParameterFormatAndCulture"/> for a resource in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/>.</summary>
        /// <param name="resourceKey">A key in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/></param>
        /// <returns>A <see cref="OneParameterFormatAndCulture"/> that can be used for formatting error messages.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="resourceKey"/> is not found in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/>.</exception>
        public static OneParameterFormatAndCulture CreateDefault(string resourceKey)
        {
            if (typeof(Properties.Resources).GetProperty(resourceKey, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) == null)
            {
                throw new ArgumentOutOfRangeException($"No resource found for key: {resourceKey}");
            }

            var formatAndCulture = new OneParameterFormatAndCulture(CultureInfo.InvariantCulture, resourceKey);
            formatAndCulture.Cache[CultureInfo.InvariantCulture] = formatAndCulture;
            return formatAndCulture;
        }

        /// <inheritdoc/>
        public override OneParameterFormatAndCulture GetOrCreate(IFormatProvider formatProvider)
        {
            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
            return this.Cache.GetOrAdd(culture, c => new OneParameterFormatAndCulture(formatProvider, this.ResourceKey));
        }

        public string FormatMessage(object arg)
        {
            return string.Format(this.FormatProvider, this.Format, arg);
        }

        /// <inheritdoc/>
        protected override void AssertFormat(IFormatProvider culture, string format)
        {
            int itemCount;
            bool? _;
            if (!FormatString.IsValidFormat(format, out itemCount, out _))
            {
                throw new InvalidOperationException($"The format: '{format}' for culture: '{culture}' is not valid.");
            }

            if (itemCount != 1)
            {
                throw new InvalidOperationException($"The format: '{format}' for culture: '{culture}' is not for one argument as expected.");
            }
        }
    }
}