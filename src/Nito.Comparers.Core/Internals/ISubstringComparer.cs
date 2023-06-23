using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Internals
{
    public interface ISubstringComparer
    {
        int GetHashCode(string source, int offset, int length);
        int Compare(string stringA, int offsetA, int lengthA, string stringB, int offsetB, int lengthB);
    }
}
