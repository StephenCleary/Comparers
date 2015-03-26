using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class FailComparer<T> : IComparer<T>, IEqualityComparer<T>
{
    public int Compare(T x, T y)
    {
        throw new Exception("FailComparer was invoked.");
    }

    public bool Equals(T x, T y)
    {
        throw new Exception("FailComparer was invoked.");
    }

    public int GetHashCode(T obj)
    {
        throw new Exception("FailComparer was invoked.");
    }
}
