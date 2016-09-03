namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>Optimized this a lot to avoid caching of results.</summary>
    internal static class FormatString
    {
        private static readonly ThreadLocal<SortedSet<int>> Indices = new ThreadLocal<SortedSet<int>>(() => new SortedSet<int>());

        /// <summary>Checks if <paramref name="format"/> has argument placeholders like 'Value: {0}'</summary>
        /// <param name="format">A format string.</param>
        /// <returns>True if the string contains format placeholders.</returns>
        public static bool IsFormatString(string format)
        {
            int count;
            bool? anyItemHasFormat;
            if (IsValidFormat(format, out count, out anyItemHasFormat))
            {
                return count != 0;
            }

            return false;
        }

        /// <summary>Check if <paramref name="format"/> is a valid format string for <paramref name="numberOfArguments"/></summary>
        /// <param name="format">The format string.</param>
        /// <param name="numberOfArguments">The number of format arguments.</param>
        /// <returns>True if <paramref name="format"/> is well formed and matches <paramref name="numberOfArguments"/></returns>
        public static bool IsValidFormatString(string format, int numberOfArguments)
        {
            if (string.IsNullOrEmpty(format))
            {
                return numberOfArguments == 0;
            }

            if (numberOfArguments < 0)
            {
                throw new ArgumentException(nameof(numberOfArguments));
            }

            int indexCount;
            bool? anyItemHasFormat;
            if (IsValidFormat(format, out indexCount, out anyItemHasFormat))
            {
                return indexCount == numberOfArguments;
            }

            return false;
        }

        /// <summary>
        /// Check a format string for errors and other properties.
        /// Does not throw nor allocate no need to cache the result as it is about as fast as a dictionary lookup for common strings.
        /// </summary>
        /// <param name="format">The format string to check</param>
        /// <param name="indexCount">The number of format indices or -1 if error</param>
        /// <param name="anyItemHasFormat">If any index has formatting i.e: {0:N}</param>
        /// <returns>True if <paramref name="format"/> is a valid format string</returns>
        internal static bool IsValidFormat(string format, out int indexCount, out bool? anyItemHasFormat)
        {
            if (string.IsNullOrEmpty(format))
            {
                indexCount = 0;
                anyItemHasFormat = false;
                return true;
            }

            var pos = 0;
            anyItemHasFormat = false;
            var indices = Indices.Value;
            indices.Clear();
            while (TrySkipTo(format, '{', '}', ref pos))
            {
                if (format[pos] == '}')
                {
                    if (TrySkipEscaped(format, '}', ref pos))
                    {
                        continue;
                    }

                    indexCount = -1;
                    return false;
                }

                if (TrySkipEscaped(format, '{', ref pos))
                {
                    continue;
                }

                int index;
                bool? itemHasFormat;
                if (!TryParseItemFormat(format, ref pos, out index, out itemHasFormat))
                {
                    indexCount = -1;
                    anyItemHasFormat = null;
                    return false;
                }

                anyItemHasFormat |= itemHasFormat;
                indices.Add(index);
            }

            if (indices.Count == 0)
            {
                indexCount = 0;
                return true;
            }

            if (indices.Min == 0 && indices.Max == indices.Count - 1)
            {
                indexCount = indices.Count;
                return true;
            }

            indexCount = -1;
            return false;
        }

        private static bool TrySkipEscaped(string text, char c, ref int pos)
        {
            if (pos < text.Length - 1 && text[pos] == c && text[pos + 1] == c)
            {
                pos += 2;
                return true;
            }

            return false;
        }

        private static bool TrySkipTo(string text, char c1, char c2, ref int pos)
        {
            while (pos < text.Length && text[pos] != c1 && text[pos] != c2)
            {
                pos++;
            }

            return pos < text.Length;
        }

        private static bool TrySkipTo(string text, char c, ref int pos)
        {
            while (pos < text.Length && text[pos] != c)
            {
                pos++;
            }

            return pos < text.Length;
        }

        private static bool TryParseItemFormat(string text, ref int pos, out int index, out bool? itemHasFormat)
        {
            if (text[pos] != '{')
            {
                index = -1;
                itemHasFormat = null;
                return false;
            }

            pos++;
            if (!TryParseUnsignedInt(text, ref pos, out index))
            {
                itemHasFormat = null;
                return false;
            }

            if (!TryParseFormatSuffix(text, ref pos, out itemHasFormat) || !TrySkipTo(text, '}', ref pos))
            {
                index = -1;
                itemHasFormat = null;
                return false;
            }

            pos++;
            return true;
        }

        private static bool TryParseFormatSuffix(string text, ref int pos, out bool? itemHasFormat)
        {
            if (pos >= text.Length)
            {
                itemHasFormat = null;
                return false;
            }

            if (text[pos] == '}')
            {
                itemHasFormat = false;
                return true;
            }

            if (text[pos] != ':')
            {
                itemHasFormat = null;
                return false;
            }

            if (pos < text.Length - 1 && text[pos + 1] == '}')
            {
                itemHasFormat = null;
                return false;
            }

            pos++;
            if (!TrySkipTo(text, '}', ref pos))
            {
                itemHasFormat = null;
                return false;
            }

            itemHasFormat = true;
            return true;
        }

        private static bool TryParseUnsignedInt(string text, ref int pos, out int result)
        {
            result = -1;
            while (pos < text.Length)
            {
                var i = text[pos] - '0';
                if (i < 0 || i > 9)
                {
                    return result != -1;
                }

                if (result == -1)
                {
                    result = i;
                }
                else
                {
                    result *= 10;
                    result += i;
                }

                pos++;
            }

            return result != -1;
        }
    }
}
