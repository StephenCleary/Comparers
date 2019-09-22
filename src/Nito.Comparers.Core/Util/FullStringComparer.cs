using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Util
{
    /// <summary>
    /// A <see cref="StringComparer"/> that forwards its implementation to an underlying <see cref="IFullComparer{T}"/>.
    /// </summary>
    internal sealed class FullStringComparer : StringComparer
    {
        private readonly IFullComparer<string> _source;

        public FullStringComparer(IFullComparer<string> source)
        {
            _source = source ?? ComparerBuilder.For<string>().Default();
        }

        public override string ToString() => _source.ToString();

        public override int Compare(string x, string y) => _source.Compare(x, y);

        public override bool Equals(string x, string y) => _source.Equals(x, y);

        public override int GetHashCode(string obj) => _source.GetHashCode(obj);
    }
}
