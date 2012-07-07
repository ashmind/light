using System;
using System.Collections.Generic;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class LiteralTests {
        [Test]
        [Row( "1",       "{1: Int32}")]
        [Row("10",       "{10: Int32}")]
        [Row("10000000", "{10000000: Int32}")]
        [Row("1.1",      "{1.1: Double}")]
        [Row("1e3",      "{1000: Double}")]
        public void Number(string literal, string expectedResult) {
            ParseAssert.IsParsedTo(literal, expectedResult, includeTypesOfValues: true);
        }

        [Test]
        [Row("'a'", "{'a': String}")]
        public void String(string literal, string expectedResult) {
            ParseAssert.IsParsedTo(literal, expectedResult, includeTypesOfValues: true);
        }

        [Test]
        [Row("[]",              "[]")]
        [Row("[1]",             "[1]")]
        [Row("[1, 'a']",        "[1, 'a']")]
        [Row("[[1], 'a', []]", "[[1], 'a', []]")]
        public void List(string literal, string expectedResult) {
            ParseAssert.IsParsedTo(literal, expectedResult);
        }

        [Test]
        [Row("{}",              "{}")]
        [Row("{a: 1}",          "{a: 1}")]
        [Row("{a: 'a'}",        "{a: 'a'}")]
        [Row("{a: [{x: 'y'}]}", "{a: [{x: 'y'}]}")]
        public void Object(string literal, string expectedResult) {
            ParseAssert.IsParsedTo(literal, expectedResult);
        }
    }
}
