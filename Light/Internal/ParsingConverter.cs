using System;
using System.Collections.Generic;
using System.Linq;
using Light.Parsing;

namespace Light.Internal {
    public static class ParsingConverter {
        public static SourceLocation FromIrony(Irony.Parsing.SourceLocation location, string source) {
            return new SourceLocation(location.Line, location.Column, location.Position, source);
        }

        public static SourceSpan FromIrony(Irony.Parsing.SourceSpan span, string source) {
            return new SourceSpan(
                FromIrony(span.Location, source),
                new SourceLocation(span.Location.Line, span.Location.Column + span.Length, span.EndPosition, source)
            );
        }
    }
}
