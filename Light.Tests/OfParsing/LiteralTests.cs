using System;
using System.Collections.Generic;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public partial class LiteralTests {
        [Test]
        [Row( "1",       "{1: Int32}")]
        [Row("10",       "{10: Int32}")]
        [Row("-1",       "{-1: Int32}")]
        [Row("10000000", "{10000000: Int32}")]
        [Row("1.1",      "{1.1: Double}")]
        [Row("1e3",      "{1000: Double}")]
        public void Number(string literal, string expectedResult) {
            var parser = new LightParser();
            var result = parser.Parse(literal);

            var visitor = new TestAstVisitor { IncludesTypesOfValues = true };
            AssertEx.That(() => !result.HasErrors);
            Assert.AreEqual(expectedResult, visitor.Describe(result.Tree));
        }
    }
}
