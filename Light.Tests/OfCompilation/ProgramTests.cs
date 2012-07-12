using System;
using System.Collections.Generic;
using System.Linq;
using Light.Compilation;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ProgramTests {
        [Test]
        public void HelloWorld_HasCorrectMainMethod() {
            var assembly = CompilationHelper.CompileFromResource("Programs.HelloWorld.light", CompilationTarget.Console);
            var main = assembly.GetTypes().Select(t => t.GetMethod("Main")).SingleOrDefault(m => m != null);

            Assert.IsNotNull(main);
            Assert.IsTrue(main.IsStatic);
            Assert.AreEqual(assembly.EntryPoint, main);
        }
    }
}
