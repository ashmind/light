using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Light.Framework;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class PrimitiveTests {
        [Test]
        [Row("1.1", 1.1)]
        [Row("1", 1)]
        [Row("'x'", "x")]
        [Row("true", true)]
        public void Simple(string valueString, object expectedValue) {
            var value = CompilationHelper.CompileAndEvaluate(valueString);
            Assert.AreEqual(TestArgumentConverter.Convert(expectedValue), value);
        }

        [Test]
        public void BigInteger() {
            const string bigIntegerString = "1000000000000000000000000000000000000000";
            var value = CompilationHelper.CompileAndEvaluate(bigIntegerString);
            Assert.AreEqual(Framework.Integer.Parse(bigIntegerString), value);
        }
    }
}
