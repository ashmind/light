using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class OperatorTests {
        [Test]
        [Row("1+1", 2)]
        [Row("1*1", 1)]
        [Row("1/1", 1)]
        [Row("1-1", 0)]
        [Row("10 mod 3", 1)]
        [Row("1>1", false)]
        [Row("1<1", false)]
        [Row("1==1", true)]
        [Row("'x'+'x'", "xx")]
        [Row("'x'=='x'", true)]
        public void SimpleConstants(string expressionString, object expectedValue) {
            var compiled = CompilationHelper.CompileAndEvaluate(expressionString);
            Assert.AreEqual(expectedValue, compiled);
        }
    }
}
