using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EqualityComparers;

namespace Comparers
{
    /// <summary>
    /// A comparer that also provides equality comparison (and hash codes) for both generic and non-generic usage.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
#if !NO_GENERIC_VARIANCE
    public interface IFullComparer<in T> : IComparer<T>, IFullEqualityComparer<T>, IFullComparer
#else
    public interface IFullComparer<T> : IComparer<T>, IFullEqualityComparer<T>, IFullComparer
#endif
    {
    }
}
