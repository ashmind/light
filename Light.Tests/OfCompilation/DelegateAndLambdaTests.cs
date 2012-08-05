using System;
using System.Collections.Generic;
using System.Linq;
using Light.Framework;
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
        public void MethodGroup_External_GenericMethod_WithoutOverloads_InMethodCall() {
            var result = CompilationHelper.CompileAndEvaluate("TestMethods.AcceptsGenericTToObject(5, TestMethods.GenericTToObject)");
            Assert.IsInstanceOfType<Func<Integer, object>>(result);
        }

        [Test]
        public void LambdaExpression_TypedParameter_Identity() {
            var result = CompilationHelper.CompileAndEvaluate("(integer x) => x");
            Assert.IsInstanceOfType<Func<Integer, Integer>>(result);
        }

        [Test]
        public void LambdaExpression_TypedParameter_Condition() {
            var result = CompilationHelper.CompileAndEvaluate("(integer x) => x > 5");
            Assert.IsInstanceOfType<Func<Integer, bool>>(result);
        }

        [Test]
        [Row("(integer x) => x")]
        [Row("x => x")]
        public void LambdaExpression_Identity_InMethodCall(string lambda) {
            var result = CompilationHelper.CompileAndEvaluate("TestMethods.AcceptsGenericTToT(7, " + lambda + ")");
            Assert.IsInstanceOfType<Func<Integer, Integer>>(result);
        }

        [Test]
        [Row("(integer x) => x > 3")]
        [Row("x => x > 3")]
        public void LambdaExpression_Condition_InMethodCall(string lambda) {
            var result = CompilationHelper.CompileAndEvaluate("[1, 2, 3, 4, 5].Where(" + lambda + ")");
            Assert.AreElementsEqual(new[] { new Integer(4), new Integer(5) }, result);
        }
    }
}
