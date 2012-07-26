using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Incomplete;
using Light.Ast.Names;
using Light.Ast.Statements;
using Light.Compilation;
using Light.Description;
using Light.Parsing;

namespace Light.Interpreter {
    public static class Program {
        private static LightParser parser;
        private static LightProcessor processor;
        private static LightCompiler compiler;
        private static TypeFormatter typeFormatter;

        public static void Main() {
            SetupCompiler();
            while (InteractiveLoop()) { /* think about world */ }
        }

        private static void SetupCompiler() {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(LightProcessor).Assembly);
            builder.RegisterAssemblyModules(typeof(LightCompiler).Assembly);
            var container = builder.Build();

            parser = container.Resolve<LightParser>();
            processor = container.Resolve<LightProcessor>();
            compiler = container.Resolve<LightCompiler>();

            typeFormatter = container.Resolve<TypeFormatter>();
        }

        private static bool InteractiveLoop() {
            Console.Write("beam> ");
            var line = Console.ReadLine();
            if (line == "exit")
                return false;

            var parsed = parser.Parse(line);
            WriteMessages(parsed);
            if (parsed.HasErrors)
                return true;

            var expression = parsed.Root as IAstExpression;
            if (expression == null) {
                var root = (AstRoot)parsed.Root;
                if (root.Elements.Count > 1 || !(root.Elements[0] is IAstExpression)) {
                    WriteLine(ConsoleColor.Red, "Expected expression, received {0} instead.", string.Join(",", root.Elements.Select(e => e.GetType().Name)));
                    return true;
                }

                expression = (IAstExpression)root.Elements[0];
            }
            
            try {
                var result = CompileAndEvaluate(expression);
                WriteLine(ConsoleColor.White, Describe(result));
            }
            catch (Exception ex) {
                WriteLine(ConsoleColor.Red, (object)ex.Message);
            }

            return true;
        }

        private static object CompileAndEvaluate(IAstExpression expression) {
            var ast = new AstRoot(
                new ImportDefinition(new CompositeName("System")),
                new ImportDefinition(new CompositeName("System", "Linq")),
                new AstTypeDefinition(
                    TypeDefintionTypes.Class, "Interactive",
                    new AstFunctionDefinition("Evaluate", No.Parameters, new[] { new AstReturnStatement(expression) }, AstImplicitType.Instance) {
                        Compilation = { Static = true }
                    }
                )
            );
            ast = (AstRoot)processor.Process(ast);

            var stream = new MemoryStream();
            compiler.Compile(ast, stream, new CompilationArguments {
                AssemblyName = "Dynamic-" + Guid.NewGuid(),
                AssemblyVersion = new Version("1.0.0.0"),
                Target = CompilationTarget.Library
            });
            var assembly = Assembly.Load(stream.ToArray()); // this obviously leaks memory
            var evaluate = assembly.GetType("Interactive", true).GetMethod("Evaluate");

            return evaluate.Invoke(null, null);
        }
        
        private static string Describe(object result) {
            if (result == null)
                return "null: Null";

            var typeString = typeFormatter.Format(result.GetType());
            var valueString = new ObjectFormatter().Format(result);

            return valueString + ": " + typeString;
        }

        private static void WriteMessages(ParsingResult parsed) {
            foreach (var message in parsed.Messages) {
                var color = ConsoleColor.White;
                if (message.IsError) {
                    color = ConsoleColor.Red;
                }
                else if (message.IsWarning) {
                    color = ConsoleColor.DarkYellow;
                }

                WriteLine(color, "{0} at {1}.", message.Text, message.Location.Column);
                Console.ResetColor();
            }
        }

        private static void WriteLine(ConsoleColor color, object arg) {
            Console.ForegroundColor = color;
            Console.WriteLine(arg);
            Console.ResetColor();
        }

        private static void WriteLine(ConsoleColor color, string format, params object[] args) {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }
    }
}
