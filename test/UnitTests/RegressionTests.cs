using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Xunit;

namespace UnitTests
{
    public class RegressionTests
    {
        [Fact]
        public void ComparableBaseDefaultComparer_WithoutTypeInstantiation_IsNotNull()
        {
            Assert.NotNull(DoNotInstantiateThisType.DefaultComparer);
        }

        [Fact]
        public void EquatableBaseDefaultComparer_WithoutTypeInstantiation_IsNotNull()
        {
            Assert.NotNull(DoNotInstantiateThisTypeEither.DefaultComparer);
        }

        private sealed class DoNotInstantiateThisType : ComparableBase<DoNotInstantiateThisType>
        {
            static DoNotInstantiateThisType() => DefaultComparer = ComparerBuilder.For<DoNotInstantiateThisType>().Null();
        }

        private sealed class DoNotInstantiateThisTypeEither : EquatableBase<DoNotInstantiateThisTypeEither>
        {
            static DoNotInstantiateThisTypeEither() => DefaultComparer = EqualityComparerBuilder.For<DoNotInstantiateThisTypeEither>().Null();
        }
    }
}
