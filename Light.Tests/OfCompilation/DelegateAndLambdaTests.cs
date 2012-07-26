using System;
using System.Collections.Generic;
using System.Linq;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class DelegateAndLambdaTests {
        [Test]
        public void MethodGroup_External_StaticMethod_WithoutOverloads() {
            var result = CompilationHelper.CompileAndEvaluate("TimeSpan.FromMinutes");
            Assert.IsInstanceOfType<Func<double, TimeSpan>>(result);
        }

        [Test]
        public void LambdaExpression_Identity() {
            var result = CompilationHelper.CompileAndEvaluate("(integer x) => x");
            Assert.IsInstanceOfType<Func<int, int>>(result);
        }
    }
}
