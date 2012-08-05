using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class MathTests {
        [Test]
        [Row(10, 10)]
        public void MathPow(double x, double y) {
            var result = CompilationHelper.CompileAndEvaluate(string.Format("Math.Pow(({0:0.00}).ToDouble(), ({1:0.00}).ToDouble())", x, y));
            Assert.AreEqual(Math.Pow(10, 10), result);
        }
    }
}
