using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Nito.Comparers.Util
{
    public sealed class StringSpanComparer : IFullComparer<string>
    {
        private readonly Func<CompareInfo> _getCompareInfo;
        private readonly CompareOptions _options;

        public StringSpanComparer(Func<CompareInfo> getCompareInfo, CompareOptions options)
        {
            _getCompareInfo = getCompareInfo;
            _options = options;
        }

        public StringSpanComparer(CompareInfo compareInfo, CompareOptions options)
            : this(() => compareInfo, options)
        {
        }

        public StringSpanComparer(StringComparison comparison)
            : this(() => GetCompareInfo(comparison), GetCompareOptions(comparison))
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

        public int Compare(ReadOnlySpan<char> x, ReadOnlySpan<char> y) => _getCompareInfo().Compare(x, y, _options);

        public bool Equals(ReadOnlySpan<char> x, ReadOnlySpan<char> y) => Compare(x, y) == 0;

        public int GetHashCode(ReadOnlySpan<char> obj) => _getCompareInfo().GetHashCode(obj, _options);

        public int Compare(string? x, string? y)
        {
            throw new NotImplementedException();
        }

        public bool Equals(string? x, string? y)
        {
            throw new NotImplementedException();
        }

        private bool DoEquals(string x, string y) => Equals(x.AsSpan(), y.AsSpan());

        public int GetHashCode(string? obj)
        {
            throw new NotImplementedException();
        }

        private bool DoGetHashCode(string obj) => GetHashCode(obj.AsSpan());

        int IComparer.Compare(object? x, object? y)
        {
            throw new NotImplementedException();
        }

        bool IEqualityComparer.Equals(object? x, object? y) => EqualityComparerHelpers.ImplementEquals<string>(x, y, false, DoEquals);

        int IEqualityComparer.GetHashCode(object? obj) => EqualityComparerHelpers.ImplementGetHashCode<string>(obj, false, DoGetHashCode);
    }
}
