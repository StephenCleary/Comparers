using System.Collections.Generic;
using System.Linq;
using Nito.Comparers;
using Xunit;
using System;

#pragma warning disable CS0162

namespace UnitTests
{
    public class Compare_BuilderUnitTests
    {
        [Fact]
        public void ForT_ReturnsBuilderForT()
        {
            var result = ComparerBuilder.For<int>();
            Assert.IsType<ComparerBuilderFor<int>>(result);
        }

        [Fact]
        public void ForFuncT_ReturnsBuilderForT_WithoutInvokingFunc()
        {
            var result = ComparerBuilder.For(() =>
            {
                throw new Exception();
                return 3;
            });
            Assert.IsType<ComparerBuilderFor<int>>(result);
        }

        [Fact]
        public void ForElementsOfT_ReturnsBuilderForT_WithoutEnumeratingSequence()
        {
            var result = ComparerBuilder.ForElementsOf(ThrowEnumerable());
            Assert.IsType<ComparerBuilderFor<int>>(result);
        }

        [Fact]
        public void ForElementsOfFuncT_ReturnsBuilderForT_WithoutInvokingFunc()
        {
            var result = ComparerBuilder.ForElementsOf(() =>
            {
                throw new Exception();
                return ThrowEnumerable();
            });
            Assert.IsType<ComparerBuilderFor<int>>(result);
        }

        private static IEnumerable<int> ThrowEnumerable()
        {
            throw new Exception();
            yield return 13;
        }
    }
}
