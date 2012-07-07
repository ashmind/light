using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace Light.Tests.OfParsing {
    public static class ParseAssert {
        public static void IsParsedTo(string literal, string expectedResult, bool includeTypesOfValues = false) {
            var parser = new LightParser();
            var result = parser.Parse(literal);

            var visitor = new TestAstVisitor { IncludesTypesOfValues = includeTypesOfValues };
            AssertEx.That(() => !result.HasErrors);
            Assert.AreEqual(expectedResult, visitor.Describe(result.Root));
        }
    }
}
