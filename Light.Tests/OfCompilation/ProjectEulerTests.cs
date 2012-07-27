using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ProjectEulerTests {
        // Please note that I am not aiming for a best solution from math perspective
        // I will always select a simpler solution that is easy to understand, to make tests more readable

        [Test]
        // improve: ranges, implicit typing in lambdas
        [Row("Enumerable.Range(1, 999).Where((integer x) => (x mod 3 == 0) or (x mod 5 == 0)).Sum()", 233168,
             Description = "Add all the natural numbers below one thousand that are multiples of 3 or 5.")]
        public void Problem(string code, object expectedValue) {
            var result = CompilationHelper.CompileAndEvaluate(code);
            Assert.AreEqual(expectedValue, result);
        }
    }
}
