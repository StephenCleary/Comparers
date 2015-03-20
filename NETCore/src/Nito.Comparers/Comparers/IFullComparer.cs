using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers
{
    /// <summary>
    /// A non-generic comparer that also provides equality comparison (and hash codes).
    /// </summary>
    public interface IFullComparer : IComparer, IEqualityComparer
    {
    }
}
