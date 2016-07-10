namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    /// <summary>A <see cref="IFormatAndCulture"/> for exactly one parameter.</summary>
    public abstract class FormatAndCulture<T> : IFormatAndCulture
        where T : IFormatAndCulture
    {
        private readonly Lazy<string> format;

        protected FormatAndCulture(IFormatProvider formatProvider, string resourceKey)
        {
            this.FormatProvider = formatProvider;
            this.ResourceKey = resourceKey;
            // lazy needed here to avoid virtual call in ctor.
            // maybe it would work any way but playing it safe.
            this.format = new Lazy<string>(() => this.GetFormat(formatProvider));
        }

        /// <summary>The name of the resource. I.e. Properties.Resources.ResourceManager.GetString(<see cref="ResourceKey"/>, <see cref="CultureInfo"/>)</summary>
        public string ResourceKey { get; }

        /// <summary>The culture for which the <see cref="Format"/> is for. If no localization is found <see cref="CultureInfo.InvariantCulture"/> will be used.</summary>
        public IFormatProvider FormatProvider { get; }

        /// <summary>Gets the localized format string.</summary>
        public string Format => this.format.Value;

        protected ConcurrentDictionary<CultureInfo, T> Cache { get; } = new ConcurrentDictionary<CultureInfo, T>();

        public abstract T GetOrCreate(IFormatProvider formatProvider);

        public string GetFormat(IFormatProvider culture)
        {
            var format = Properties.Resources.ResourceManager.GetString(this.ResourceKey, culture as CultureInfo);
            if (!string.IsNullOrEmpty(format))
            {
                this.AssertFormat(culture, format);
                return format;
            }

            return Properties.Resources.ResourceManager.GetString(this.ResourceKey, CultureInfo.InvariantCulture);
        }

        /// <summary>Assert that <paramref name="format"/> is valid.</summary>
        /// <param name="culture">The culture.</param>
        /// <param name="format">The format string.</param>
        /// <exception cref="InvalidOperationException">If the format does not match the expected.</exception>
        protected abstract void AssertFormat(IFormatProvider culture, string format);
    }
}