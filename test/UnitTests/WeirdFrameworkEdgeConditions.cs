using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Xunit;

namespace UnitTests
{
    public class WeirdFrameworkEdgeConditions
    {
        [Fact]
        public void DefaultEqualityComparer_ForValueType_IEqualityComparer_UnexpectedTypes()
        {
            // When accessing the default equality comparer via the nongeneric IEqualityComparer interface...
            IEqualityComparer frameworkComparer = EqualityComparer<int>.Default;
            IEqualityComparer nitoComparer = ComparerBuilder.For<int>().Default();
            var obj1 = new object();

            // The framework version returns true if any reference-equal instances are passed in...
            //   https://github.com/dotnet/corefx/blob/53a33cf2662ac8c9a45d13067012d80cf0ba6956/src/Common/src/CoreLib/System/Collections/Generic/EqualityComparer.cs#L29
            Assert.True(frameworkComparer.Equals(obj1, obj1));
            
            // But throws if that same object is passed to GetHashCode.
            Assert.Throws<ArgumentException>(() => frameworkComparer.GetHashCode(obj1));

            // It also throws if different instances are passed in that are not the expected type.
            Assert.Throws<ArgumentException>(() => frameworkComparer.Equals(obj1, new object()));

            // Nito Comparers throw in all scenarios for consistency.
            Assert.Throws<ArgumentException>(() => nitoComparer.Equals(obj1, obj1));
            Assert.Throws<ArgumentException>(() => nitoComparer.GetHashCode(obj1));
            Assert.Throws<ArgumentException>(() => nitoComparer.Equals(obj1, new object()));
        }
    }
}
