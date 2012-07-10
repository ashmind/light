using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Light.Ast;
using Light.Compilation;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ProgramTests {
        [Test]
        public void HelloWorld() {
            AssertIsCompiled("HelloWorld.light");
        }

        private static void AssertIsCompiled(string programName) {
            var code = Resource.ReadAllText(typeof(Resource), "Programs." + programName);
            var parsed = new LightParser().Parse(code);
            new LightProcessor().Process(parsed.Root);

            var compilationArguments = new CompilationArguments {
                AssemblyName = "Test",
                AssemblyVersion = new Version(0, 1),
                Target = CompilationTarget.Console
            };

            var stream = new MemoryStream();
            new LightCompiler().Compile(stream, (AstRoot)parsed.Root, compilationArguments);

            Assert.DoesNotThrow(() => Assembly.Load(stream.ToArray()));
        }
    }
}
