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
        public void Binary(string code, string expectedResult) {
            var parser = new LightParser();
            var result = parser.Parse(code);

            AssertEx.That(() => !result.HasErrors
                 && new TestAstVisitor().Describe(result.Tree) == expectedResult);
        }
    }
}
