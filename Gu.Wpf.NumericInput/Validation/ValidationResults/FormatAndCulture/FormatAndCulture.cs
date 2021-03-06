namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;

    /// <summary>Metadata about a validation error message.</summary>
    /// <typeparam name="T">The specific type.</typeparam>
    public abstract class FormatAndCulture<T> : IFormatAndCulture
        where T : IFormatAndCulture
    {
        private readonly Lazy<string> format;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatAndCulture{T}"/> class.
        /// </summary>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/>.</param>
        /// <param name="resourceKey">The <see cref="string"/>.</param>
        protected FormatAndCulture(IFormatProvider formatProvider, string resourceKey)
        {
            this.FormatProvider = formatProvider;
            this.ResourceKey = resourceKey;

            // used lazy here to avoid virtual call in constructor.
            // maybe it would work any way but playing it safe.
            this.format = new Lazy<string>(() => this.GetFormat(formatProvider));
        }

        /// <inheritdoc/>
        public string ResourceKey { get; }

        /// <inheritdoc/>
        public IFormatProvider FormatProvider { get; }

#pragma warning disable CA1721 // Property names should not match get methods
        /// <inheritdoc/>
        public string Format => this.format.Value;
#pragma warning restore CA1721 // Property names should not match get methods

        /// <summary>
        /// Gets the cache.
        /// </summary>
        protected ConcurrentDictionary<CultureInfo, T> Cache { get; } = new ConcurrentDictionary<CultureInfo, T>();

        /// <summary>
        /// Get from cache or create a new instance.
        /// </summary>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/>.</param>
        /// <returns>A <typeparamref name="T"/>.</returns>
        public abstract T GetOrCreate(IFormatProvider formatProvider);

        /// <inheritdoc/>
        public string GetFormat(IFormatProvider culture)
        {
            var formatString = Properties.Resources.ResourceManager.GetString(this.ResourceKey, culture as CultureInfo);
            if (!string.IsNullOrEmpty(formatString))
            {
                this.AssertFormat(culture, formatString);
                return formatString;
            }

            return Properties.Resources.ResourceManager.GetString(this.ResourceKey, CultureInfo.InvariantCulture) ?? $"No format found for key: {this.ResourceKey}.";
        }

        /// <summary>Assert that <paramref name="format"/> is valid.</summary>
        /// <param name="culture">The culture.</param>
        /// <param name="format">The format string.</param>
        /// <exception cref="InvalidOperationException">If the format does not match the expected.</exception>
        protected abstract void AssertFormat(IFormatProvider culture, string format);
    }
}
