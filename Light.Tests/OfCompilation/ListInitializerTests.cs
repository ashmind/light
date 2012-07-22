using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ListInitializerTests {
        [Test]
        [Row("[1]",        new object[] { 1 })]
        [Row("[1, 2]",     new object[] { 1, 2 })]
        [Row("['x', 'y']", new object[] { "x", "y" })]
        public void Simple(string expressionString, params object[] expectedValues) {
            var compiled = ((Array)CompilationHelper.CompileAndEvaluate(expressionString)).Cast<object>().ToArray();
            Assert.AreElementsEqual(expectedValues, compiled);
        }
    }
}
