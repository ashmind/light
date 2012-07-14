using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

        public static dynamic CompileCodeAndGetInstance(string code, string typeName) {
            var assembly = CompileCode(code);
            var type = assembly.GetType(typeName, true);

            return Activator.CreateInstance(type);
        }

        public static Assembly CompileCode(string code, CompilationTarget target = CompilationTarget.Library) {
            var parsed = new LightParser().Parse(code);
            AssertEx.That(() => !parsed.HasErrors);

            var processor = TestEnvironment.Container.Resolve<LightProcessor>();
            processor.Process(parsed.Root);
            
            var assemblyName = GetAssemblyName();
            var compilationArguments = new CompilationArguments {
                AssemblyName = assemblyName,
                AssemblyVersion = new Version(0, 1, 0, 0),
                Target = target
            };

            var stream = new MemoryStream();
            var compiler = TestEnvironment.Container.Resolve<LightCompiler>();
            compiler.Compile((AstRoot)parsed.Root, stream, compilationArguments);

            // for debugging
            WriteAssemblyOnDiskForDebugging(stream, assemblyName, target);
            return Assembly.Load(stream.ToArray());
        }

        private static string GetAssemblyName() {
            var stepName = TestContext.CurrentContext.TestStep.Name;
            var stepNameCleanedUp = Regex.Replace(stepName, @"[^\w\d]+", ".").Trim('.');
            return stepNameCleanedUp;
        }

        // you should be able to find assemblies in c:\Users\<user>\AppData\Local\Temp\Light.Tests\
        private static void WriteAssemblyOnDiskForDebugging(MemoryStream stream, string assemblyName, CompilationTarget target) {
            var debuggingHelperDirectory = Path.Combine(Path.GetTempPath(), "Light.Tests");
            Directory.CreateDirectory(debuggingHelperDirectory);

            var extension = target == CompilationTarget.Console ? ".exe" : ".dll";
            File.WriteAllBytes(Path.Combine(debuggingHelperDirectory, assemblyName + extension), stream.ToArray());
        }
    }
}
