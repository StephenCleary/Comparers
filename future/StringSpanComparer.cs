using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A type that can compare strings as well as read-only spans of characters.
    /// </summary>
    public sealed class StringSpanComparer : IFullComparer<string>
    {
        private readonly CompareInfo _compareInfo;
        private readonly CompareOptions _options;

        /// <summary>
        /// Creates a new instance using the specified compare info and options.
        /// </summary>
        public StringSpanComparer(CompareInfo compareInfo, CompareOptions options)
        {
            _compareInfo = compareInfo;
            _options = options;
        }

        /// <summary>
        /// Creates a new instance using the specified string comparison.
        /// </summary>
        public StringSpanComparer(StringComparison comparison)
            : this(GetCompareInfo(comparison), GetCompareOptions(comparison))
        {
        }

        private static CompareInfo GetCompareInfo(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.CurrentCulture => CultureInfo.CurrentCulture.CompareInfo,
                StringComparison.CurrentCultureIgnoreCase => CultureInfo.CurrentCulture.CompareInfo,
                (StringComparison)2 /* InvariantCulture */ => CultureInfo.InvariantCulture.CompareInfo,
                (StringComparison)3 /* InvariantCultureIgnoreCase */ => CultureInfo.InvariantCulture.CompareInfo,
                StringComparison.Ordinal => CultureInfo.InvariantCulture.CompareInfo,
                StringComparison.OrdinalIgnoreCase => CultureInfo.InvariantCulture.CompareInfo,
                _ => throw new ArgumentException($"The string comparison type {comparison} is not supported.", nameof(comparison)),
            };
        }

        private static CompareOptions GetCompareOptions(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.CurrentCulture => CompareOptions.None,
                StringComparison.CurrentCultureIgnoreCase => CompareOptions.IgnoreCase,
                (StringComparison)2 /* InvariantCulture */ => CompareOptions.None,
                (StringComparison)3 /* InvariantCultureIgnoreCase */ => CompareOptions.IgnoreCase,
                StringComparison.Ordinal => CompareOptions.Ordinal,
                StringComparison.OrdinalIgnoreCase => CompareOptions.OrdinalIgnoreCase,
                _ => throw new ArgumentException($"The string comparison type {comparison} is not supported.", nameof(comparison)),
            };
        }

        /// <summary>
        /// Compares two read-only spans of characters as though they were strings.
        /// </summary>
        public int Compare(ReadOnlySpan<char> x, ReadOnlySpan<char> y) => _compareInfo.Compare(x, y, _options);

        /// <summary>
        /// Determines whether two read-only spans of characters are equal, as though they were strings.
        /// </summary>
        public bool Equals(ReadOnlySpan<char> x, ReadOnlySpan<char> y) => Compare(x, y) == 0;

        /// <summary>
        /// Gets a hash code for a read-only span of characters, as though it were a string.
        /// </summary>
        public int GetHashCode(ReadOnlySpan<char> obj) => _compareInfo.GetHashCode(obj, _options);

        /// <inheritdoc />
        public int Compare(string? x, string? y)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Equals(string? x, string? y) => EqualityComparerHelpers.ImplementEquals(x, y, false, DoEquals!);

        private bool DoEquals(string? x, string? y) => Equals(x == null ? default : x.AsSpan(), y == null ? default : y.AsSpan());

        /// <inheritdoc />
        public int GetHashCode(string? obj) => EqualityComparerHelpers.ImplementGetHashCode(obj, false, DoGetHashCode!);

        private int DoGetHashCode(string? obj) => GetHashCode(obj == null ? default : obj.AsSpan());

        int IComparer.Compare(object? x, object? y)
        {
            throw new NotImplementedException();
        }

        bool IEqualityComparer.Equals(object? x, object? y) => EqualityComparerHelpers.ImplementEquals<string>(x, y, false, DoEquals);

        int IEqualityComparer.GetHashCode(object? obj) => EqualityComparerHelpers.ImplementGetHashCode<string>(obj, false, DoGetHashCode);
    }
}
