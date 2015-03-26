using System.Collections;

namespace Nito.Comparers
{
    /// <summary>
    /// A non-generic comparer that also provides equality comparison (and hash codes).
    /// </summary>
    public interface IFullComparer : IComparer, IEqualityComparer
    {
    }
}
