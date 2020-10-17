using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers.Util;

namespace Nito.Comparers.Advanced
{
    /// <summary>
    /// Allows defining advanced comparers with custom logic.
    /// </summary>
    /// <typeparam name="T">The type of objects to be compared.</typeparam>
    public abstract class AdvancedComparerBase<T> : IFullComparer<T>
    {
        private readonly Implementation _implementation;

        /// <summary>
        /// Initializes a new instance of this type using standard <c>null</c> handling semantics.
        /// </summary>
        protected AdvancedComparerBase()
            : this(specialNullHandling: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of this type.
        /// </summary>
        /// <param name="specialNullHandling">A value indicating whether <c>null</c> values are passed to <see cref="DoGetHashCode"/> and <see cref="DoCompare"/>. If <c>false</c>, then <c>null</c> values are considered less than any non-<c>null</c> values and are not passed to <see cref="DoGetHashCode"/> nor <see cref="DoCompare"/>. This value is ignored if <typeparamref name="T"/> is a non-nullable type.</param>
        protected AdvancedComparerBase(bool specialNullHandling)
        {
            _implementation = new Implementation(specialNullHandling, this);
        }

        /// <summary>
        /// Gets a value indicating whether <c>null</c> values will be passed down to derived implementations.
        /// This is equal to the <c>specialNullHandling</c> value passed to the constructor, except for when <typeparamref name="T"/> is not nullable, in which case this value is always <c>false</c>.
        /// </summary>
        protected bool SpecialNullHandling => _implementation.SpecialNullHandlingValue;

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code. May be <c>null</c> if <see cref="SpecialNullHandling"/> is <c>true</c>.</param>
        /// <returns>A hash code for the specified object.</returns>
        protected abstract int DoGetHashCode(T obj);

        /// <summary>
        /// Compares two objects and returns a value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first object to compare. May be <c>null</c> if <see cref="SpecialNullHandling"/> is <c>true</c>.</param>
        /// <param name="y">The second object to compare. May be <c>null</c> if <see cref="SpecialNullHandling"/> is <c>true</c>.</param>
        /// <returns>A value less than 0 if <paramref name="x"/> is less than <paramref name="y"/>, 0 if <paramref name="x"/> is equal to <paramref name="y"/>, or greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
        protected abstract int DoCompare(T x, T y);

        /// <inheritdoc />
        public int Compare(T x, T y) => ((IComparer<T>)_implementation).Compare(x, y);

        /// <inheritdoc />
        public bool Equals(T x, T y) => ((IEqualityComparer<T>)_implementation).Equals(x, y);

        /// <inheritdoc />
        public int GetHashCode(T obj) => ((IEqualityComparer<T>)_implementation).GetHashCode(obj);

        int IComparer.Compare(object x, object y) => ((IComparer)_implementation).Compare(x, y);

        bool IEqualityComparer.Equals(object x, object y) => ((IEqualityComparer)_implementation).Equals(x, y);

        int IEqualityComparer.GetHashCode(object obj) => ((IEqualityComparer)_implementation).GetHashCode(obj);

        private sealed class Implementation : ComparerBase<T>
        {
            private readonly AdvancedComparerBase<T> _parent;

            public Implementation(bool specialNullHandling, AdvancedComparerBase<T> parent)
                : base(specialNullHandling)
            {
                _parent = parent;
            }

            public bool SpecialNullHandlingValue => SpecialNullHandling;

            protected override int DoGetHashCode(T obj) => _parent.DoGetHashCode(obj);

            protected override int DoCompare(T x, T y) => _parent.DoCompare(x, y);
        }
    }
}
