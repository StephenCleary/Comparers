namespace Nito.Comparers.Util
{
    /// <summary>
    /// The null comparer.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    internal sealed class NullComparer<T> : ComparerBase<T>
    {
        private NullComparer()
            : base(true)
        {
        }

        static NullComparer()
        {
            // This type constructor does nothing; it only exists to make static field initialization deterministic.
        }

        private static readonly NullComparer<T> instance = new NullComparer<T>();

        /// <summary>
        /// Gets the null comparer for this type.
        /// </summary>
        public static NullComparer<T> Instance
        {
            get
            {
                return instance;
            }
        }

        /// <inheritdoc />
        protected override int DoGetHashCode(T obj)
        {
            return unchecked((int)2166136261);
        }

        /// <inheritdoc />
        protected override int DoCompare(T x, T y)
        {
            return 0;
        }

        /// <summary>
        /// Returns a short, human-readable description of the comparer. This is intended for debugging and not for other purposes.
        /// </summary>
        public override string ToString()
        {
            return "Null";
        }
    }
}
