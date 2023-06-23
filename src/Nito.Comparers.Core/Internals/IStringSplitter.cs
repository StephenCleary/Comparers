using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStringSplitter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="isNumeric"></param>
        /// <returns></returns>
        void MoveNext(string source, ref int offset, out int length, out bool isNumeric);
    }
}
