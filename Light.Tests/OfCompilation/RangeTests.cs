using System;
using System.Collections.Generic;
using System.Linq;
using Light.Framework;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class RangeTests {
        [Test]
        [Row("1..10", 1, 10)]
        public void SimpleInteger(string expressionString, int expectedFrom, int expectedTo) {
            var compiled = CompilationHelper.CompileAndEvaluate(expressionString);
            Assert.AreElementsEqual(
                Enumerable.Range(expectedFrom, expectedTo - expectedFrom).Select(x => new Integer(x)),
                compiled
            );
        }
    }
}
