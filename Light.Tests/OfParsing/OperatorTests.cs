using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    [TestFixture]
    public class OperatorTests {
        [Test]
        [Row("1+2", "{1 + 2}")]
        [Row("1-2", "{1 - 2}")]
        [Row("1*2", "{1 * 2}")]
        [Row("1/2", "{1 / 2}")]
        [Row("1+\n2", "{1 + 2}")]
        public void Binary(string code, string expectedResult) {
            ParseAssert.IsParsedTo(code, expectedResult);
        }
    }
}
