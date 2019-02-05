using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Util
{
    public static class DataUtility
    {
        /// <summary>
        /// Creates a new string instance with the same characters.
        /// </summary>
        // ReSharper disable once UseStringInterpolation
        public static string Duplicate(string data) => string.Format("{0}", data);
    }
}
