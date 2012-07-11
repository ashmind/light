using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Gallio.Framework;
using Light.Ast;
using Light.Compilation;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    public static class CompilationHelper {
        public static Assembly CompileFromResource(string resourceName, CompilationTarget target = CompilationTarget.Library) {
            var code = Resource.ReadAllText(typeof(Resource), resourceName);
            return CompileCode(code, target);
        }

        public static Assembly CompileCode(string code, CompilationTarget target = CompilationTarget.Library) {
            var parsed = new LightParser().Parse(code);
            AssertEx.That(() => !parsed.HasErrors);

            new LightProcessor().Process(parsed.Root);

            var compilationArguments = new CompilationArguments {
                AssemblyName = "TestAssembly",
                AssemblyVersion = new Version(0, 1),
                Target = CompilationTarget.Console
            };

            var stream = new MemoryStream();
            var compiler = TestEnvironment.Container.Resolve<LightCompiler>();
            compiler.Compile((AstRoot)parsed.Root, stream, compilationArguments);

            // for debugging
            WriteAssemblyOnDiskForDebugging(stream);
            return Assembly.Load(stream.ToArray());
        }

        private static void WriteAssemblyOnDiskForDebugging(MemoryStream stream) {
            var debuggingHelperDirectory = Path.Combine(Path.GetTempPath(), "Light.Tests");
            Directory.CreateDirectory(debuggingHelperDirectory);

            File.WriteAllBytes(Path.Combine(debuggingHelperDirectory, TestContext.CurrentContext.Test.Name + "-TestAssembly.dll"), stream.ToArray());
        }
    }
}
