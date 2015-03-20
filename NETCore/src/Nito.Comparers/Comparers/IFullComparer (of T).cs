using System;
using System.Collections.Generic;
using System.Text;
using Nito.EqualityComparers;

namespace Nito.Comparers
{
    /// <summary>
    /// A comparer that also provides equality comparison (and hash codes) for both generic and non-generic usage.
    /// </summary>
    /// <typeparam name="T">The type of objects being compared.</typeparam>
    public interface IFullComparer<in T> : IComparer<T>, IFullEqualityComparer<T>, IFullComparer
    {
    }
}
