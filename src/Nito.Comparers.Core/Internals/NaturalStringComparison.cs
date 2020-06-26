using Nito.Comparers.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Nito.Comparers.Internals
{
    /// <summary>
    /// Implements "natural" string comparison.
    /// </summary>
    public static class NaturalStringComparison
    {
        private static readonly char[] Digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Gets a hash code for the specified string, using the specified string comparison for the text segments.
        /// </summary>
        /// <param name="obj">The string to calculate the hash value of. May not be <c>null</c>.</param>
        /// <param name="comparison">The string comparison to use for the text segments.</param>
        public static int GetHashCode(string obj, StringComparison comparison)
        {
            int index = 0;
            var result = Murmur3Hash.Create();
            while (index < obj.Length)
            {
                var start = index;
                NextSegment(obj, ref start, out var end, out var isNumeric);
                
                // Note that leading zeros have been stripped from the range [start, end), so an ordinal comparison is sufficient to detect numeric equality.
                var segmentComparison = isNumeric ? StringComparison.Ordinal : comparison;

#if NETSTANDARD1_0 || NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_0 || NET45
                var segmentHashCode = TryGetComparer(segmentComparison)?.GetHashCode(obj.Substring(start, end - start)) ?? 0;
#else
                var segmentHashCode = string.GetHashCode(obj.AsSpan(start, end - start), segmentComparison);
#endif

                result.Combine(segmentHashCode);
                index = end;
            }

            return result.HashCode;
        }

        // Implementation map:
        // .NET Core 3.0+ - This method is not defined. string.GetHashCode is used instead.
        // .NET Standard 2.1+ - This method forwards to StringComparer.FromComparison.
        // .NET Standard 2.0 - This method is a switch statement, supporting all StringComparison values.
        // .NET Standard 1.0-1.6 - This method is a switch statement and does not support invariant cultures.
        //   This can be a problem for Xamarin.Android 7.1, Xamarin.iOS 10.8, and Xamarin.Mac 3.0, all of which have invariant comparers but do not support .NET Standard 2.0.
        //   The recommended solution on those platforms is "upgrade to a .NET Standard 2.0-compatible version".
        // .NET Core 2.0-2.2 - This method forwards to StringComparer.FromComparison.
        // .NET Framework 4.5+ - This method is a switch statement, supporting all StringComparison values.
#if NETSTANDARD1_0 || NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_0 || NET45
#if !NETSTANDARD1_0 && !NETSTANDARD2_0 && !NET45
        private static StringComparer? TryGetComparer(StringComparison comparison)
        {
            try
            {
                return StringComparer.FromComparison(comparison);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
#else
        private static StringComparer? TryGetComparer(StringComparison comparison)
        {
            switch (comparison)
            {
                case StringComparison.Ordinal: return StringComparer.Ordinal;
                case StringComparison.OrdinalIgnoreCase: return StringComparer.OrdinalIgnoreCase;
                case StringComparison.CurrentCulture: return StringComparer.CurrentCulture;
                case StringComparison.CurrentCultureIgnoreCase: return StringComparer.CurrentCultureIgnoreCase;
#if !NETSTANDARD1_0
                case StringComparison.InvariantCulture: return StringComparer.InvariantCulture;
                case StringComparison.InvariantCultureIgnoreCase: return StringComparer.InvariantCultureIgnoreCase;
#endif
                default: return null;
            }
        }
#endif
#endif

        /// <summary>
        /// Compares the specified strings, using the specified string comparison for the text segments.
        /// </summary>
        /// <param name="x">The first string to compare. May not be <c>null</c>.</param>
        /// <param name="y">The first string to compare. May not be <c>null</c>.</param>
        /// <param name="comparison">The string comparison to use for the text segments.</param>
        public static int Compare(string x, string y, StringComparison comparison)
        {
            int xIndex = 0, yIndex = 0;
            while (xIndex < x.Length && yIndex < y.Length)
            {
                var xStart = xIndex;
                var yStart = yIndex;
                NextSegment(x, ref xStart, out var xEnd, out var xIsNumeric);
                NextSegment(y, ref yStart, out var yEnd, out var yIsNumeric);
                if (xIsNumeric && yIsNumeric)
                {
                    var xLength = xEnd - xStart;
                    var yLength = yEnd - yStart;
                    if (xLength < yLength)
                        return -1;
                    else if (xLength > yLength)
                        return 1;
                    var compareResult = string.Compare(x, xStart, y, yStart, xLength, StringComparison.Ordinal);
                    if (compareResult != 0)
                        return compareResult;
                }
                else if (!xIsNumeric && !yIsNumeric)
                {
                    var xLength = xEnd - xStart;
                    var yLength = yEnd - yStart;
                    var compareResult = Compare(x, xStart, xLength, y, yStart, yLength, comparison);
                    if (compareResult != 0)
                        return compareResult;
                    var lengthCompare = xLength - yLength;
                    if (lengthCompare != 0)
                        return lengthCompare;
                }
                else if (xIsNumeric)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }

                xIndex = xEnd;
                yIndex = yEnd;
            }

            if (xIndex < x.Length)
                return 1;
            if (yIndex > y.Length)
                return -1;
            return 0;
        }

        private static int Compare(string strA, int indexA, int lengthA, string strB, int indexB, int lengthB, StringComparison comparisonType)
        {
            // Blatantly stolen from https://dogmamix.com/cms/blog/Finding-substrings
            return comparisonType switch
            {
                StringComparison.CurrentCulture => CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None),
                StringComparison.CurrentCultureIgnoreCase => CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase),
                (StringComparison)2 /* InvariantCulture */ => CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None),
                (StringComparison)3 /* InvariantCultureIgnoreCase */ => CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase),
                StringComparison.Ordinal => CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.Ordinal),
                StringComparison.OrdinalIgnoreCase => CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.OrdinalIgnoreCase),
                _ => throw new ArgumentException($"The string comparison type {comparisonType} is not supported.", nameof(comparisonType)),
            };
        }

        private static void NextSegment(string source, ref int start, out int end, out bool isNumeric)
        {
            // Prerequisite: index < source.Length
            var index = start;
            isNumeric = IsDigit(source[index++]);
            if (isNumeric)
            {
                // Skip leading zeros, but keep one if that's the only digit.
                if (source[start] == '0')
                {
                    do
                    {
                        ++start;
                    } while (start < source.Length && source[start] == '0');
                    if (start == source.Length || !IsDigit(source[start]))
                        --start;
                    index = start + 1;
                }

                while (index < source.Length && IsDigit(source[index]))
                    ++index;
                end = index;
            }
            else
            {
                index = source.IndexOfAny(Digits, index);
                if (index == -1)
                    index = source.Length;
                end = index;
            }

            bool IsDigit(char ch) => ch >= '0' && ch <= '9';
        }
    }
}
