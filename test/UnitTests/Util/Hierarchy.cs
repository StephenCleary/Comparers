using System;
using System.Collections.Generic;
using System.Text;
using Nito.Comparers;

namespace UnitTests.Util
{
    public class HierarchyBase
    {
        public int Id { get; set; }
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

        public static IFullEqualityComparer<HierarchyDerived1> Derived1EqualityComparerDefinedForBase =>
            EqualityComparerBuilder.For<HierarchyBase>().EquateBy(x => x.Id);

        public static IFullComparer<HierarchyDerived1> Derived1ComparerDefinedForBase =>
            ComparerBuilder.For<HierarchyBase>().OrderBy(x => x.Id);
    }
}
