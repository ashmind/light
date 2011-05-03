using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Irony.Parsing;
using Light.Parsing;

namespace Light {
    public class LightParser {
        private readonly Parser parser;

        public LightParser() {
            this.parser = new Parser(new LightGrammar());
        }

        public ParsingResult Parse(string source) {
            var parsed = this.parser.Parse(source);
            return new ParsingResult(
                parsed.Root != null ? (Expression)parsed.Root.AstNode : null,
                parsed.ParserMessages.Select(ToParsingMessage)
            );
        }

        private static ParsingMessage ToParsingMessage(ParserMessage ironyMessage) {
            return new ParsingMessage(
                ironyMessage.Message,
                (ParsingMessageKind)ironyMessage.Level,
                new Parsing.SourceLocation(ironyMessage.Location.Column)
            );
        }
    }
}
