using System.Collections;
using System.Collections.Generic;

namespace Nito.EqualityComparers
{
    /// <summary>
    /// An equality comparer that supports both generic and non-generic equality comparison.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public interface IFullEqualityComparer<in T> : IEqualityComparer<T>, IEqualityComparer
    {
    }
}
