using System;
using System.Collections.Generic;
using System.Linq;
using Light.Framework;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ListInitializerTests {
        [Test]
        [Row("[1]",        new object[] { 1 })]
        [Row("[true]",     new object[] { true })]
        [Row("[false]",    new object[] { false })]
        [Row("[1, 2]",     new object[] { 1, 2 })]
        [Row("['x', 'y']", new object[] { "x", "y" })]
        [Row("[1.0, 2.0]", new object[] { 1.0, 2.0 })]
        [Row("[[1], [2]]", new object[] { new[] {1}, new[] {2} })]
        public void Simple(string expressionString, object[] expectedValues) {
            var compiled = ((Array)CompilationHelper.CompileAndEvaluate(expressionString)).Cast<object>().ToArray();
            Assert.AreElementsEqual(ExpectedValueConverter.Convert(expectedValues), compiled);
        }
    }
}
