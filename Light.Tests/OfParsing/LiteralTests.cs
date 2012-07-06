using System;
using System.Collections.Generic;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class LiteralTests {
        [Test]
        [Row( "1",       "{1: Int32}")]
        [Row("10",       "{10: Int32}")]
        [Row("-1",       "{-1: Int32}")]
        [Row("10000000", "{10000000: Int32}")]
        [Row("1.1",      "{1.1: Double}")]
        [Row("1e3",      "{1000: Double}")]
        public void Number(string literal, string expectedResult) {
            AssertIsParsedTo(literal, expectedResult, includeTypesOfValues: true);
        }

        [Test]
        [Row("'a'", "{'a': String}")]
        public void String(string literal, string expectedResult) {
            AssertIsParsedTo(literal, expectedResult, includeTypesOfValues: true);
        }

        [Test]
        [Row("[]",              "[]")]
        [Row("[1]",             "[1]")]
        [Row("[1, 'a']",        "[1, 'a']")]
        [Row("[[1], 'a', []]", "[[1], 'a', []]")]
        public void List(string literal, string expectedResult) {
            AssertIsParsedTo(literal, expectedResult);
        }

        private static void AssertIsParsedTo(string literal, string expectedResult, bool includeTypesOfValues = false) {
            var parser = new LightParser();
            var result = parser.Parse(literal);

            var visitor = new TestAstVisitor { IncludesTypesOfValues = includeTypesOfValues };
            AssertEx.That(() => !result.HasErrors);
            Assert.AreEqual(expectedResult, visitor.Describe(result.Tree));
        }
    }
}
