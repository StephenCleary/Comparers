using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;
using Xunit.Abstractions;

namespace UnitTests.Util
{
    public class HierarchyBase: IXunitSerializable
    {
        public int Id { get; set; }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Id = info.GetValue<int>("id");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("id", Id);
        }
    }

    public sealed class HierarchyDerived1 : HierarchyBase { }
    public sealed class HierarchyDerived2 : HierarchyBase { }

    public static class HierarchyComparers
    {
        public static IFullEqualityComparer<HierarchyBase> BaseEqualityComparer =>
            EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id);

        public static IFullComparer<HierarchyBase> BaseComparer =>
            ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id);

        public static IFullEqualityComparer<HierarchyDerived1> Derived1EqualityComparer =>
            EqualityComparerBuilder.For<HierarchyDerived1>().EquateBy(x => x.Id);

        public static IFullComparer<HierarchyDerived1> Derived1Comparer =>
            ComparerBuilder.For<HierarchyDerived1>().OrderBy(x => x.Id);
    }
}
