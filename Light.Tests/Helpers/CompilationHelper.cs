using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac;
using Gallio.Common.Reflection;
using Gallio.Framework;
using Light.Ast;
using Light.Compilation;
using Light.Framework;
using MbUnit.Framework;

namespace Light.Tests.Helpers {
    public static class CompilationHelper {
        private static readonly Assembly[] ReferencedAssemblies = {
            typeof(Range<>).Assembly
        };

        public static dynamic CompileAndEvaluate(string expression) {
            return CompileAndGetInstance(string.Format(@"
                import System   
                import System.Linq
                public class Test
                    public function Evaluate()
                        return {0}
                    end
                end
            ", expression).Trim(), "Test").Evaluate();
        }

        public static Assembly CompileFromResource(string resourceName, CompilationTarget target = CompilationTarget.Library) {
            var code = Resource.ReadAllText(typeof(Resource), resourceName);
            return CompileCode(code, target);
        }

        public static dynamic CompileAndGetInstance(string code, string typeName) {
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

            var bytes = stream.ToArray();
            var assembly = Assembly.Load(bytes);
            WriteAssemblyOnDiskAndPEVerify(assembly, bytes, target);
            return assembly;
        }

        private static string GetAssemblyName() {
            var stepName = TestContext.CurrentContext.TestStep.Name;
            var stepNameCleanedUp = Regex.Replace(stepName, @"[^\w\d\.]+", "_").Trim('_');
            return stepNameCleanedUp;
        }

        // you should be able to find assemblies in c:\Users\<user>\AppData\Local\Temp\Light.Tests\
        private static void WriteAssemblyOnDiskAndPEVerify(Assembly assembly, byte[] bytes, CompilationTarget target) {
            var parent = TestContext.CurrentContext.Parent;
            while (parent.Test.CodeElement.Kind != CodeElementKind.Type) {
                parent = parent.Parent;
            }

            var outputDirectory = Path.Combine(Path.GetTempPath(), "Light.Tests", parent.Test.Name);
            Directory.CreateDirectory(outputDirectory);

            var extension = target == CompilationTarget.Console ? ".exe" : ".dll";

            var path = Path.Combine(outputDirectory, assembly.GetName().Name + extension);
            File.WriteAllBytes(path, bytes);

            foreach (var referenced in ReferencedAssemblies) {
                var referencePath = Path.Combine(outputDirectory, Path.GetFileName(referenced.Location));
                if (!File.Exists(referencePath))
                    File.Copy(referenced.Location, referencePath);
            }

            PEVerifier.Verify(path, assembly);
        }
    }
}
