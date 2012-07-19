using System;
using System.Collections.Generic;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class LambdaTests {
        [Test]
        [Row("x => x", "x => x")]
        [Row("(a, b) => a",  "(a, b) => a")]
        [Row("(int x) => x", "(int x) => x")]
        public void SingleLineNonNested(string code, string parsed) {
            ParseAssert.IsParsedTo(code, parsed);
        }

        [Test]
        [Row("x => y => x + y", "x => y => x + y")]
        [Row("x => (int y) => x", "x => (int y) => x")]
        public void SingleLineNested(string code, string parsed) {
            ParseAssert.IsParsedTo(code, parsed);
        }
    }
}
