using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Xunit;
// ReSharper disable ReturnValueOfPureMethodIsNotUsed

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

            // The framework version returns true if any reference-equal instances are passed in
            //   https://github.com/dotnet/corefx/blob/53a33cf2662ac8c9a45d13067012d80cf0ba6956/src/Common/src/CoreLib/System/Collections/Generic/EqualityComparer.cs#L29
            Assert.True(frameworkComparer.Equals(obj1, obj1));
            
            // But throws if that same object is passed to GetHashCode
            Assert.ThrowsAny<ArgumentException>(() => frameworkComparer.GetHashCode(obj1));

            // It also throws if different instances are passed in that are not the expected type
            Assert.ThrowsAny<ArgumentException>(() => frameworkComparer.Equals(obj1, new object()));

            // Nito Comparers throw in all scenarios for consistency
            Assert.ThrowsAny<ArgumentException>(() => nitoComparer.Equals(obj1, obj1));
            Assert.ThrowsAny<ArgumentException>(() => nitoComparer.GetHashCode(obj1));
            Assert.ThrowsAny<ArgumentException>(() => nitoComparer.Equals(obj1, new object()));
        }

        [Fact]
        public void StringComparer_Null()
        {
            var frameworkStringComparer = StringComparer.Ordinal;
            var frameworkDefaultComparer = EqualityComparer<string>.Default;
            var nitoStringComparer = ComparerBuilder.For<string>().OrderBy(x => x, StringComparer.Ordinal);

            // The string comparer allows equating to null
            Assert.True(frameworkStringComparer.Equals(null, null));
            Assert.False(frameworkStringComparer.Equals("test", null));

            // But throws if null is passed to GetHashCode
            //   https://github.com/dotnet/corefx/blob/36e812b0e974fa113c50be71e1701735a8a63481/src/Common/src/CoreLib/System/StringComparer.cs#L150-L153
            Assert.ThrowsAny<ArgumentException>(() => frameworkStringComparer.GetHashCode(null));

            // The framework default comparer does not throw
            Assert.True(frameworkDefaultComparer.Equals(null, null));
            Assert.False(frameworkDefaultComparer.Equals("test", null));
            frameworkDefaultComparer.GetHashCode(null);

            // And neither does the Nito string comparer wrapper.
            Assert.True(nitoStringComparer.Equals(null, null));
            Assert.False(nitoStringComparer.Equals("test", null));
            nitoStringComparer.GetHashCode(null);
        }
    }
}
