using System.Runtime.CompilerServices;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// The reference equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class ReferenceEqualityComparer<T> : EqualityComparerBase<T>
        where T : class
    {
        private ReferenceEqualityComparer()
            : base(false)
        {
        }

        static ReferenceEqualityComparer()
        {
            // This type constructor does nothing; it only exists to make static field initialization deterministic.
        }

        /// <summary>
        /// Gets the reference comparer for this type.
        /// </summary>
        public static ReferenceEqualityComparer<T> Instance { get; } = new ReferenceEqualityComparer<T>();

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj) => RuntimeHelpers.GetHashCode(obj);

        /// <inheritdoc />
        protected override bool DoEquals(T x, T y) => object.ReferenceEquals(x, y);

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString() => "Reference";
    }
}
