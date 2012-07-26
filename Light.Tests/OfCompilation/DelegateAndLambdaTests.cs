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
        public void LambdaExpression_TypedParameter_Identity() {
            var result = CompilationHelper.CompileAndEvaluate("(integer x) => x");
            Assert.IsInstanceOfType<Func<int, int>>(result);
        }

        [Test]
        public void LambdaExpression_TypedParameter_Condition() {
            var result = CompilationHelper.CompileAndEvaluate("(integer x) => x > 5");
            Assert.IsInstanceOfType<Func<int, bool>>(result);
        }

        [Test]
        public void LambdaExpression_TypedParameter_InMethodCall() {
            var result = CompilationHelper.CompileAndEvaluate("[1, 2, 3, 4, 5].Where((integer x) => x > 3)");
            Assert.AreElementsEqual(new[] { 4, 5 }, result);
        }
    }
}
