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
        [Ignore("NotImplementedYet")]
        public void LambdaExpression_UntypedParameter_Identity() {
            var result = CompilationHelper.CompileAndEvaluate("x => x");
            Assert.IsInstanceOfType<Func<int, bool>>(result);
        }

        [Test]
        [Row("(integer x) => x > 3")]
        //[Row("x => x > 3")]
        public void LambdaExpression_InMethodCall(string lambda) {
            var result = CompilationHelper.CompileAndEvaluate("[1, 2, 3, 4, 5].Where(" + lambda + ")");
            Assert.AreElementsEqual(new[] { 4, 5 }, result);
        }
    }
}
