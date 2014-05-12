using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EqualityComparers
{
    /// <summary>
    /// An equality comparer that supports both generic and non-generic equality comparison.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
#if !NO_GENERIC_VARIANCE
    public interface IFullEqualityComparer<in T> : IEqualityComparer<T>, IEqualityComparer
#else
    public interface IFullEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
#endif
    {
    }
}
