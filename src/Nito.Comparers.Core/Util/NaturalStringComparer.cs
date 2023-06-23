using Nito.Comparers.Internals;
using System;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// The natural string comparer.
    /// </summary>
    internal sealed class NaturalStringComparer : ComparerBase<string>
    {
        private readonly Func<string, int, int, string, int, int, int> _substringCompare;
        private readonly Func<string, int, int, int> _substringGetHashCode;

        public NaturalStringComparer(StringComparison comparison)
            : base(false)
        {
            _substringCompare = NaturalStringComparison.GetSubstringCompare(comparison);
            _substringGetHashCode = NaturalStringComparison.GetSubstringGetHashCode(comparison);
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(string? obj) => NaturalStringComparison.GetHashCode(obj!, _substringGetHashCode);

        /// <inheritdoc />
        protected override int DoCompare(string? x, string? y) => NaturalStringComparison.Compare(x!, y!, _substringCompare);

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => "NaturalString";
    }
}
