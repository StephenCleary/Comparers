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
    public interface IFullComparer<in T> : IComparer<T>, IEqualityComparer<T>, IFullComparer
    {
    }
}
