using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class OperatorTests {
        [Test]
        [Row("1+2",     "1 + 2")]
        [Row("1-2",     "1 - 2")]
        [Row("1*2",     "1 * 2")]
        [Row("1/2",     "1 / 2")]
        [Row("1 mod 2", "1 mod 2")]
        [Row("1+\n2",   "1 + 2")]
        public void Binary_Math(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }

        [Test]
        [Row("1 == 1", "1 == 1")]
        public void Binary_Comparison(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }

        [Test]
        [Row("true or false", "true or false")]
        public void Binary_Boolean(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }

        [Test]
        [Row("10 mod 5 == 2",   "((10 mod 5) == 2)")]
        [Row("2 ** 4 - 2 ** 3", "((2 ** 4) - (2 ** 3))")]
        public void Binary_Precedence(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult, parenthiseAll: true);
        }
    }
}
