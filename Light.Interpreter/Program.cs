using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Light.Parsing;

namespace Light.Interpreter {
    public static class Program {
        public static void Main() {
            var parser = new LightParser();

            while (InterpretLoop(parser)) { /* think about world */ }
        }

        private static bool InterpretLoop(LightParser parser) {
            Console.Write("beam> ");
            var line = Console.ReadLine();
            if (line == "exit")
                return false;

            var parsed = parser.Parse(line);
            WriteMessages(parsed);
            if (parsed.HasErrors)
                return true;

            var expression = parsed.Tree;
            var func = Expression.Lambda<Func<object>>(
                Expression.Convert(expression, typeof(object))
            ).Compile();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(func());
            Console.ResetColor();

            return true;
        }

        private static void WriteMessages(ParsingResult parsed) {
            foreach (var message in parsed.Messages) {
                if (message.IsError) {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (message.IsWarning) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }

                Console.WriteLine("{0} at {1}.", message.Text, message.Location.Column);
                Console.ResetColor();
            }
        }
    }
}
