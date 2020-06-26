using Nito.Comparers.Internals;
using System;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// The natural string comparer.
    /// </summary>
    internal sealed class NaturalStringComparer : ComparerBase<string>
    {
        private readonly StringComparison _comparison;

        public NaturalStringComparer(StringComparison comparison)
            : base(false)
        {
            _comparison = comparison;
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(string obj) => NaturalStringComparison.GetHashCode(obj, _comparison);

        /// <inheritdoc />
        protected override int DoCompare(string x, string y) => NaturalStringComparison.Compare(x, y, _comparison);

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => "NaturalString";
    }
}
