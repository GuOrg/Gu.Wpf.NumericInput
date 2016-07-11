namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Reflection;

    public class TwoParameterFormatAndCulture : FormatAndCulture<TwoParameterFormatAndCulture>
    {
        private TwoParameterFormatAndCulture(IFormatProvider formatProvider, string resourceKey)
            : base(formatProvider, resourceKey)
        {
        }

        /// <summary>Create a <see cref="TwoParameterFormatAndCulture"/> for a resource in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/>.</summary>
        /// <param name="resourceKey">A key in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/></param>
        /// <returns>A <see cref="TwoParameterFormatAndCulture"/> that can be used for formatting error messages.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="resourceKey"/> is not found in <see cref="Gu.Wpf.NumericInput.Properties.Resources"/>.</exception>
        public static TwoParameterFormatAndCulture CreateDefault(string resourceKey)
        {
            if (typeof(Properties.Resources).GetProperty(resourceKey, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) == null)
            {
                throw new ArgumentOutOfRangeException($"No resource found for key: {resourceKey}");
            }

            var formatAndCulture = new TwoParameterFormatAndCulture(CultureInfo.InvariantCulture, resourceKey);
            formatAndCulture.Cache[CultureInfo.InvariantCulture] = formatAndCulture;
            return formatAndCulture;
        }

        /// <inheritdoc/>
        public override TwoParameterFormatAndCulture GetOrCreate(IFormatProvider formatProvider)
        {
            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
            return this.Cache.GetOrAdd(culture, c => new TwoParameterFormatAndCulture(formatProvider, this.ResourceKey));
        }

        public string FormatMessage(object arg1, object arg2)
        {
            return string.Format(this.FormatProvider, this.Format, arg1, arg2);
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

            if (itemCount != 2)
            {
                throw new InvalidOperationException($"The format: '{format}' for culture: '{culture}' is not for two arguments as expected.");
            }
        }
    }
}