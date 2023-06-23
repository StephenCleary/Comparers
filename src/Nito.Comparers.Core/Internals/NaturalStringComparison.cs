﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#pragma warning disable IDE0079
#pragma warning disable IDE0057

namespace Nito.Comparers.Internals
{
    /// <summary>
    /// Implements "natural" string comparison.
    /// </summary>
    public static class NaturalStringComparison
    {
        private static readonly char[] Digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static Func<string, int, int, int> OrdinalStringGetHashCode = GetSubstringGetHashCode(StringComparison.Ordinal);

        /// <summary>
        /// Gets a hash code for the specified string, using the specified string comparison for the text segments.
        /// </summary>
        /// <param name="obj">The string to calculate the hash value of. May not be <c>null</c>.</param>
        /// <param name="substringGetHashCode">The string delegate used to get the hash code of the text segments (not used for numeric segments).</param>
        public static int GetHashCode(string obj, Func<string, int, int, int> substringGetHashCode)
        {
            _ = obj ?? throw new ArgumentNullException(nameof(obj));
            _ = substringGetHashCode ?? throw new ArgumentNullException(nameof(substringGetHashCode));

            var parser = new SegmentParser(obj);
            var result = Murmur3Hash.Create();
            while (!parser.IsDone)
            {
                parser.ParseNext();
                
                // Note that leading zeros have been stripped from the range [start, end), so an ordinal comparison is sufficient to detect numeric equality.
                var segmentGetHashCode = parser.IsNumeric ? OrdinalStringGetHashCode : substringGetHashCode;
                var segmentHashCode = segmentGetHashCode(parser.Source, parser.Start, parser.Length);

                result.Combine(segmentHashCode);
            }

            return result.HashCode;
        }

        /// <summary>
        /// Returns a delegate that performs a substring hash code using the specified comparision type.
        /// </summary>
        /// <param name="stringComparison">The comparison type used by the returned delegate.</param>
        public static Func<string, int, int, int> GetSubstringGetHashCode(StringComparison stringComparison)
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_0 || NET45 || NET461
            var comparer = TryGetComparer(stringComparison);
            if (comparer == null)
                return (str, offset, length) => 0;
            return (str, offset, length) => comparer.GetHashCode(str.Substring(offset, length));
#else
            return (str, offset, length) => string.GetHashCode(str.AsSpan(offset, length), stringComparison);
#endif

            // Implementations, in order of preference:
            //  1) Use string.GetHashCode (.NET Core 3.0+).
            //  2) Forward to StringComparer.FromComparison (.NET Core 2.0-2.2, .NET Standard 2.1+).
            //  3) Use a switch statement that supports all StringComparison values (.NET Framework 4.5+, .NET Standard 2.0).
            //  4) Use a switch statement that doesn't support the invariant culture (.NET Standard 1.0-1.6).
            // By platform:
            //  .NET Core 3.0+ - This method is not defined. string.GetHashCode is used instead.
            //  .NET Core 2.0-2.2 - This method forwards to StringComparer.FromComparison.
            //  .NET Framework 4.5+ - This method is a switch statement, supporting all StringComparison values.
            //  .NET Standard 2.1+ - This method forwards to StringComparer.FromComparison.
            //  .NET Standard 2.0 - This method is a switch statement, supporting all StringComparison values.
            //  .NET Standard 1.0-1.6 - This method is a switch statement and does not support invariant cultures.
            //    This can be a problem for Xamarin.Android 7.1, Xamarin.iOS 10.8, and Xamarin.Mac 3.0, all of which have invariant comparers but do not support .NET Standard 2.0.
            //    This will cause tons of hash code collisions.
            //    The recommended solution on those platforms is "upgrade to a .NET Standard 2.0-compatible version".
#if NETSTANDARD1_0 || NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_0 || NET45 || NET461
#if !NETSTANDARD1_0 && !NETSTANDARD2_0 && !NET45 && !NET461
            static StringComparer? TryGetComparer(StringComparison comparison)
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
            static StringComparer? TryGetComparer(StringComparison comparison)
            {
                return comparison switch
                {
                    StringComparison.Ordinal => StringComparer.Ordinal,
                    StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
                    StringComparison.CurrentCulture => StringComparer.CurrentCulture,
                    StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
#if !NETSTANDARD1_0
                    StringComparison.InvariantCulture => StringComparer.InvariantCulture,
                    StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
#endif
                    _ => null,
                };
            }
#endif
#endif
        }

        /// <summary>
        /// Compares the specified strings, using the specified string comparison for the text segments.
        /// </summary>
        /// <param name="x">The first string to compare. May not be <c>null</c>.</param>
        /// <param name="y">The first string to compare. May not be <c>null</c>.</param>
        /// <param name="substringCompare">The string delegate used to compare the text segments (not used for numeric segments).</param>
        public static int Compare(string x, string y, Func<string, int, int, string, int, int, int> substringCompare)
        {
            _ = x ?? throw new ArgumentNullException(nameof(x));
            _ = y ?? throw new ArgumentNullException(nameof(y));
            _ = substringCompare ?? throw new ArgumentNullException(nameof(substringCompare));

            var xParser = new SegmentParser(x);
            var yParser = new SegmentParser(y);
            while (!xParser.IsDone && !yParser.IsDone)
            {
                xParser.ParseNext();
                yParser.ParseNext();
                if (xParser.IsNumeric && yParser.IsNumeric)
                {
                    var xLength = xParser.Length;
                    var yLength = yParser.Length;
                    if (xLength < yLength)
                        return -1;
                    else if (xLength > yLength)
                        return 1;
                    var compareResult = string.Compare(xParser.Source, xParser.Start, yParser.Source, yParser.Start, xLength, StringComparison.Ordinal);
                    if (compareResult != 0)
                        return compareResult;
                }
                else if (!xParser.IsNumeric && !yParser.IsNumeric)
                {
                    var xLength = xParser.Length;
                    var yLength = yParser.Length;
                    var compareResult = substringCompare(xParser.Source, xParser.Start, xLength, yParser.Source, yParser.Start, yLength);
                    if (compareResult != 0)
                        return compareResult;
                }
                else if (xParser.IsNumeric)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }

            if (xParser.IsDone && yParser.IsDone)
                return 0;

            if (xParser.IsDone)
                return -1;
            return 1;
        }

        /// <summary>
        /// Returns a delegate that performs a substring comparison using the specified comparision type.
        /// </summary>
        /// <param name="stringComparison">The comparison type used by the returned delegate.</param>
        public static Func<string, int, int, string, int, int, int> GetSubstringCompare(StringComparison stringComparison)
        {
            // Blatantly stolen from https://dogmamix.com/cms/blog/Finding-substrings
            return stringComparison switch
            {
                StringComparison.CurrentCulture => (strA, indexA, lengthA, strB, indexB, lengthB) => CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None),
                StringComparison.CurrentCultureIgnoreCase => (strA, indexA, lengthA, strB, indexB, lengthB) => CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase),
                (StringComparison)2 /* InvariantCulture */ => (strA, indexA, lengthA, strB, indexB, lengthB) => CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.None),
                (StringComparison)3 /* InvariantCultureIgnoreCase */ => (strA, indexA, lengthA, strB, indexB, lengthB) => CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.IgnoreCase),
                StringComparison.Ordinal => (strA, indexA, lengthA, strB, indexB, lengthB) => CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.Ordinal),
                StringComparison.OrdinalIgnoreCase => (strA, indexA, lengthA, strB, indexB, lengthB) => CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, lengthA, strB, indexB, lengthB, CompareOptions.OrdinalIgnoreCase),
                _ => throw new ArgumentException($"The string comparison type {stringComparison} is not supported.", nameof(stringComparison)),
            };
        }

        private sealed class DefaultSubstringComparer : ISubstringComparer
        {
            private readonly Func<CultureInfo> _getCultureInfo;
            private readonly CompareOptions _compareOptions;

            public DefaultSubstringComparer(StringComparison comparison)
            {
                _getCultureInfo = comparison switch
                {
                    StringComparison.CurrentCulture => () => CultureInfo.CurrentCulture,
                    StringComparison.CurrentCultureIgnoreCase => () => CultureInfo.CurrentCulture,
                    _ => () => CultureInfo.InvariantCulture,
                };
            }

            public int Compare(string stringA, int inclusiveStartA, int exclusiveEndA, string stringB, int inclusiveStartB, int exclusiveEndB)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(string source, int inclusiveStart, int exclusiveEnd)
            {
                throw new NotImplementedException();
            }
        }

        private ref struct SegmentParser
        {
            public SegmentParser(string source)
            {
                Source = source;
                Start = 0;
                End = 0;
                IsNumeric = false;
            }

            public string Source { get; }
            public int Start { get; private set; }
            public int End { get; private set; }
            public int Length => End - Start;
            public bool IsDone => End == Source.Length;
            public bool IsNumeric { get; private set; }

            public void ParseNext()
            {
                // Prerequisite: index < source.Length

                Start = End;
                var index = Start;
                IsNumeric = IsDigit(Source[index++]);
                if (IsNumeric)
                {
                    // Skip leading zeros, but keep one if that's the only digit.
                    if (Source[Start] == '0')
                    {
                        do
                        {
                            ++Start;
                        } while (Start < Source.Length && Source[Start] == '0');
                        if (Start == Source.Length || !IsDigit(Source[Start]))
                            --Start;
                        index = Start + 1;
                    }

                    while (index < Source.Length && IsDigit(Source[index]))
                        ++index;
                    End = index;
                }
                else
                {
                    index = Source.IndexOfAny(Digits, index);
                    if (index == -1)
                        index = Source.Length;
                    End = index;
                }

                static bool IsDigit(char ch) => ch >= '0' && ch <= '9';
            }
        }

        private sealed class DefaultSplitter : IStringSplitter
        {
            public void MoveNext(string source, ref int offset, out int length, out bool isNumeric)
            {
                // Prerequisite: start < source.Length

                var index = offset;
                isNumeric = IsDigit(source[index++]);
                if (isNumeric)
                {
                    // Skip leading zeros, but keep one if that's the only digit.
                    if (source[offset] == '0')
                    {
                        do
                        {
                            ++offset;
                        } while (offset < source.Length && source[offset] == '0');
                        if (offset == source.Length || !IsDigit(source[offset]))
                            --offset;
                        index = offset + 1;
                    }

                    while (index < source.Length && IsDigit(source[index]))
                        ++index;
                    length = index - offset;
                }
                else
                {
                    index = source.IndexOfAny(Digits, index);
                    if (index == -1)
                        index = source.Length;
                    length = index - offset;
                }

                static bool IsDigit(char ch) => ch >= '0' && ch <= '9';
            }
        }
    }
}
