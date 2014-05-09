using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comparers
{
    /// <summary>
    /// A comparer that also provides equality comparison (and hash codes).
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
#if NO_GENERIC_VARIANCE
    public interface IFullComparer<T> : IComparer<T>, IEqualityComparer<T>, System.Collections.IComparer, System.Collections.IEqualityComparer
#else
    public interface IFullComparer<in T> : IComparer<T>, IEqualityComparer<T>, System.Collections.IComparer, System.Collections.IEqualityComparer
#endif
    {
    }
}
