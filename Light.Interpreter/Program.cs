using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Irony.Parsing;

namespace Light.Interpreter {
    public class Program {
        public static void Main() {
            var parser = new Parser(new LightGrammar());

            while (InterpretLoop(parser)) { /* think about world */ }
        }

        private static bool InterpretLoop(Parser parser) {
            Console.Write("beam> ");
            var line = Console.ReadLine();
            if (line == "exit")
                return false;

            var parsed = parser.Parse(line);
            WriteMessages(parsed);
            if (parsed.HasErrors())
                return true;

            var expression = (Expression)parsed.Root.AstNode;
            var func = Expression.Lambda<Func<object>>(
                Expression.Convert(expression, typeof(object))
            ).Compile();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(func());
            Console.ResetColor();

            return true;
        }

        private static void WriteMessages(ParseTree parsed) {
            foreach (var message in parsed.ParserMessages) {
                if (message.Level == ParserErrorLevel.Error) {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (message.Level == ParserErrorLevel.Warning) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }

                Console.WriteLine("{0} at {1}.", message.Message, message.Location.Column);
                Console.ResetColor();
            }
        }
    }
}
