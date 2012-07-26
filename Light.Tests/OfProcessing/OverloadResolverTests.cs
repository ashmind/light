using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.Ast;
using Light.Ast.Literals;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;
using Light.Processing.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfProcessing {
    [TestFixture]
    public class OverloadResolverTests {
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedParameter.Local
        private class TestClass {
            public static void NoOverloadsInt32(int x) {}

            public static void TwoOverloadsStringObject(string x) { }
            public static void TwoOverloadsStringObject(object x) { }

            public class A { }
            public class B : A { }
            public class C : B { }

            public static void TwoOverloadsAB(A a) { }
            public static void TwoOverloadsAB(B b) { }

            public static void GenericWithEnumerable<T>(IEnumerable<T> x) { }
        }
        // ReSharper restore UnusedParameter.Local
        // ReSharper restore UnusedMember.Local

        [Test]
        public void StaticNonGenericMethod_WithNoOverloads_PassingExactType() {
            var result = Resolve("NoOverloadsInt32", null, 1);
            Assert.AreEqual("NoOverloadsInt32", result.Name);
        }

        [Test]
        public void StaticGenericMethod_WithOverloads_WithSingleArgument_PassingExactTypeForFirstOverload() {
            var result = Resolve("TwoOverloadsStringObject", null, "x");

            Assert.AreEqual("TwoOverloadsStringObject", result.Name);
            Assert.AreEqual(typeof(string), result.GetParameters()[0].ParameterType);
        }

        [Test]
        public void StaticGenericMethod_WithSubclassOverloads_WithSingleArgument_PassingMoreSpecificSubclass()
        {
            var result = Resolve("TwoOverloadsAB", null, new TestClass.C());

            Assert.AreEqual("TwoOverloadsAB", result.Name);
            Assert.AreEqual(typeof(TestClass.B), result.GetParameters()[0].ParameterType);
        }

        [Test]
        public void StaticGenericMethod_WithNoOverloads_WithSingleArgument_HavingTypeParameterInSecondLevel_PassingInterfaceImplementation() {
            var result = Resolve("GenericWithEnumerable", null, new[] { 1, 2 });

            Assert.AreEqual("GenericWithEnumerable", result.Name);
            Assert.AreEqual(typeof(int), result.GetGenericArguments()[0]);
        }

        private MethodInfo Resolve(string name, object target, params object[] arguments) {
            var resolver = new OverloadResolver();
            var methods = typeof(TestClass).GetMethods().Where(m => m.Name == name);
            var result = resolver.ResolveMethodGroup(
                new AstMethodGroup(name, methods.Select(m => new AstReflectedMethod(m)).ToArray()),
                target != null ? (IAstElement)new PrimitiveValue(target) : new AstReflectedType(typeof(TestClass)),
                arguments.Select(a => new PrimitiveValue(a)).ToArray()
            );

            return ((AstReflectedMethod)result).Method;
        }
    }
}
