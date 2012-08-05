using System;
using System.Collections.Generic;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class LiteralTests {
        [Test]
        [Row( "1",       "1: integer")]
        [Row("10",       "10: integer")]
        [Row("10000000", "10000000: integer")]
        [Row("1.1",      "1.1: decimal")]
        [Row("1e3",      "1000: decimal")]
        [Row("1000000000000000000000000000000000000000000000000000000000000000000000000000",
             "1000000000000000000000000000000000000000000000000000000000000000000000000000: integer")]
        public void Number(string literal, string expectedResult) {
            ParseAssert.IsParsedTo(literal, expectedResult, includeExpressionType: true);
        }

        [Test]
        [Row("'a'", "'a': string")]
        public void String(string literal, string expectedResult) {
            ParseAssert.IsParsedTo(literal, expectedResult, includeExpressionType: true);
        }

        [Test]
        [Row("true",  "true: boolean")]
        [Row("false", "false: boolean")]
        public void Boolean(string literal, string expectedResult) {
            ParseAssert.IsParsedTo(literal, expectedResult, includeExpressionType: true);
        }

        [Test]
        [Row("1..10", "1..10")]
        public void Range(string literal, string expectedResult) {
            ParseAssert.IsParsedTo(literal, expectedResult);
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
