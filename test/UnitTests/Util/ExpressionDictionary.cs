using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace UnitTests.Util
{
    public sealed class ExpressionDictionary<T> : Dictionary<string, T>
    {
        public void Add<TExpression>(Expression<Func<TExpression>> expression)
            where TExpression : T
            => this[DataUtility.Key(expression)] = expression.Compile()();

        public TheoryData<string> AllKeys()
        {
            var result = new TheoryData<string>();
            foreach (var key in Keys)
                result.Add(key);
            return result;
        }
    }
}
