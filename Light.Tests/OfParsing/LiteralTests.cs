using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Pex.Framework;
using MbUnit.Framework;
using System.Linq.Expressions;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public partial class LiteralTests {
        [Test]
        [Row( "1",  1)]
        [Row("10", 10)]
        [Row("-1", -1)]
        [Row("10000000", 10000000)]
        public void Number(string literal, int expectedResult) {
            var parser = new LightParser();
            var result = parser.Parse(literal);

            AssertEx.That(() => !result.HasErrors
                             && (int)(result.Tree as ConstantExpression).Value == expectedResult);
        }
    }
}
