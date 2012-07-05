using System;
using System.Collections.Generic;
using System.Linq;

using Light.Interpretation;
using Light.Parsing;

namespace Light.Interpreter {
    public static class Program {
        public static void Main() {
            var parser = new LightParser();
            var interpreter = new LightInterpreter();

            while (InterpretLoop(parser, interpreter)) { /* think about world */ }
        }

        private static bool InterpretLoop(LightParser parser, LightInterpreter interpreter) {
            Console.Write("beam> ");
            var line = Console.ReadLine();
            if (line == "exit")
                return false;

            var parsed = parser.Parse(line);
            WriteMessages(parsed);
            if (parsed.HasErrors)
                return true;

            var elements = parsed.Tree;
            try {
                var result = interpreter.Evaluate(elements);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(result);
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
            }
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
