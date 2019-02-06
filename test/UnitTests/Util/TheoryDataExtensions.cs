using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests.Util
{
    public static class TheoryDataExtensions
    {
        public static TheoryData<T> ToTheoryData<T>(this IEnumerable<T> values)
        {
            var result = new TheoryData<T>();
            foreach (var value in values)
                result.Add(value);
            return result;
        }

        public static TheoryData<T1, T2> ToTheoryData<T1, T2>(this IEnumerable<(T1, T2)> values)
        {
            var result = new TheoryData<T1, T2>();
            foreach (var (item1, item2) in values)
                result.Add(item1, item2);
            return result;
        }

        public static TheoryData<T1, T2, T3> ToTheoryData<T1, T2, T3>(this IEnumerable<(T1, T2, T3)> values)
        {
            var result = new TheoryData<T1, T2, T3>();
            foreach (var (item1, item2, item3) in values)
                result.Add(item1, item2, item3);
            return result;
        }
    }
}
