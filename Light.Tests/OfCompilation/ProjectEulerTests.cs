using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ProjectEulerTests {
        // Please note that I am not aiming for a best solution from math perspective
        // What I am trying to achieve is a simple solution that is easy to understand.

        [Test]
        [Ignore] // requires: ranges, extension methods, lambdas
        [Row("0..999", 233168,
             Description = "Add all the natural numbers below one thousand that are multiples of 3 or 5.")]
        public void Problem(string code, object expectedValue) {
            var result = CompilationHelper.CompileAndEvaluate(code);
            Assert.AreEqual(expectedValue, result);
        }
    }
}
